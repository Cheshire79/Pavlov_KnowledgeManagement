using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Services.Interfaces;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Interface;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using Rotativa.MVC;
using WebUI.Mapper;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.UsersAndRoles;
using WebUI.Models.UsersSearch;

namespace WebUI.Controllers
{
    public class SearchForUsersController : Controller
    {
        private IUserService<SkillDTO, SubSkillDTO, SpecifyingSkillDTO, LevelDTO, SpecifyingSkillForSearchDTO> _userService;
        private IIdentityService _identityService;
        private IMapper _mapper;

        public SearchForUsersController(IUserService<SkillDTO, SubSkillDTO, SpecifyingSkillDTO, LevelDTO, SpecifyingSkillForSearchDTO> userService, IIdentityService identityService, IMapperFactoryWEB mapperFactory)
        {
            _userService = userService;
            _identityService = identityService;
            _mapper = mapperFactory.CreateMapperWEB();
        }
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Index()
        {
            SpecifyingSkillsForSearchViewModel specifyingSkillsViewModel = new SpecifyingSkillsForSearchViewModel();
            int minLevelId = (await _userService.GetLevels().OrderBy(x => x.Order).FirstAsync()).Id; // todo to service ?
            // GetIdOfMinimumKnowledgeLevel

            specifyingSkillsViewModel.LevelsViewModel = _mapper.Map<IEnumerable<LevelDTO>, IEnumerable<LevelViewModel>>
                (await _userService.GetLevels().OrderBy(x => x.Order).ToListAsync());

            specifyingSkillsViewModel.SpecifyingSkills =
                from s in _userService.Skill().ProjectTo<SkillViewModel>(_mapper.ConfigurationProvider)
                join sk in _userService.SubSkill().ProjectTo<SubSkillViewModel>(_mapper.ConfigurationProvider)
                    on s.Id equals sk.SkillId into g
                select new SpecifyingSkillForSearchViewModel()
                {
                    Skill = s,
                    SubSkills = g.Select(x => new SpecifyingSubSkillForSearchViewModel()
                    {
                        SubSkill = x,
                        LevelId = minLevelId,
                        OrHigher = false
                    })
                };
            //specifyingSkillsViewModel1.SpecifyingSkills =
            //    from s in _userService.Skill()
            //    join sk in _userService.SubSkill()
            //        on s.Id equals sk.SkillId into g
            //    select new SpecifyingSkillForSearchViewModel()
            //    {
            //        SkillViewModel =
            //            new SkillViewModel()
            //            {
            //                Id = s.Id,
            //                Name = s.Name
            //            },
            //        SubSkillListViewModel = g.Select(x => new SpecifyingSubSkillForSearchViewModel()
            //        {
            //            SubSkillViewModel = new SubSkillViewModel()
            //            {
            //                Id = x.Id,
            //                SkillId = x.SkillId,
            //                Name = x.Name
            //            },
            //            LevelId = minLevelId,
            //            OrHigher = false
            //        })
            //    };
            //ProjectTo<SubSkillViewModel>(_mapper.ConfigurationProvider)
            return View(specifyingSkillsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Index(List<SpecifyingSkillForSearchSaveModel> specifyingSkillsForSearchSaveModel, UsersSearchResult usersSearchResult)
        {
            UsersSearchResultViewModel usersSearchResultViewModel = new UsersSearchResultViewModel();
            if (ModelState.IsValid)
            {
                int idForMinLevelValue = await _userService.GetIdForMinLevelValue();
                // clear search criteria
                specifyingSkillsForSearchSaveModel.RemoveAll(x => x.LevelId == idForMinLevelValue && !x.OrHigher);
                usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel = specifyingSkillsForSearchSaveModel;
                usersSearchResultViewModel.UserSearchListResultViewModel = new List<UserSearchResultViewModel>();

                var specifyingSkillForSearchDTO = _mapper.Map<IEnumerable<SpecifyingSkillForSearchSaveModel>, IEnumerable<SpecifyingSkillForSearchDTO>>
                    (specifyingSkillsForSearchSaveModel);
                var usersId = _userService.GetUsersIdByCriteria(specifyingSkillForSearchDTO);

                IEnumerable<UserViewModel> Users = from id in usersId
                                                   join user in _identityService.GetUsers().ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                                                       on id equals user.Id
                                                   select user;

                foreach (var u in Users)
                {
                    usersSearchResultViewModel.UserSearchListResultViewModel.Add(new UserSearchResultViewModel()
                    {
                        User = new UserViewModel()
                        {
                            Id = u.Id,
                            Name = u.Name
                        },
                        SpecifyingSkills = (from s in _userService.Skill().ProjectTo<SkillViewModel>(_mapper.ConfigurationProvider)
                                            join osk in
                                                (from sk in _userService.SubSkill().ProjectTo<SubSkillViewModel>(_mapper.ConfigurationProvider)
                                                 join spk in _userService.GetSpecifyingSkills().Where(x => x.UserId == u.Id)
                                                     on sk.Id equals spk.SubSkillId
                                                 join lvl in _userService.GetLevels()
                                                     on spk.LevelId equals lvl.Id
                                                 select new SpecifyingSubSkillViewModel
                                                 {
                                                     SubSkill = sk,
                                                     LevelId = spk.LevelId
                                                 })
                                                on s.Id equals osk.SubSkill.SkillId into g
                                            select new SpecifyingSkillViewModel()
                                            {
                                                Skill = s,
                                                SpecifyingSubSkills = g.ToList()
                                            }).ToList()
                    });
                }
            }
            // save search result on client side to pass them to PrintSearch
            usersSearchResult.SpecifyingSkillsForSearch =
                usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel;
            usersSearchResult.Users =
                usersSearchResultViewModel.UserSearchListResultViewModel;
            return View("SearchResult", usersSearchResultViewModel);
        }
        [Authorize(Roles = "manager")]
        public ActionResult PrintSearchResult(UsersSearchResult usersSearchResult)
        {
            UsersSearchResultViewModel usersSearchResultViewModel = new UsersSearchResultViewModel();
            usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel = usersSearchResult.SpecifyingSkillsForSearch;
            usersSearchResultViewModel.UserSearchListResultViewModel = usersSearchResult.Users;
            var report = new ViewAsPdf("_SearchResultPartial", usersSearchResultViewModel);
            return report;
        }
        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            _identityService.Dispose();
            base.Dispose(disposing);
        }
    }
}
