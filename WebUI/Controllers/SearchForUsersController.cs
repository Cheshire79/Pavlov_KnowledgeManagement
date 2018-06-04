using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BLL.Identity.Services.Interfaces;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using Rotativa.MVC;
using WebUI.Mapper;
using WebUI.Models;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.SearchForUsers;

namespace WebUI.Controllers
{
    public class SearchForUsersController : Controller
    {
        private IUserService _userService;
        private IIdentityService _identityService;
        private IMapper _mapper;
       
        public SearchForUsersController(IUserService userService, IIdentityService identityService, IMapperFactoryWEB mapperFactory)
        {
            _userService = userService;
            _identityService = identityService;
            _mapper = mapperFactory.CreateMapperWEB();
        }
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Index()
        {
            SpecifyingSkillsForSearchViewModel specifyingSkillsViewModel1 = new SpecifyingSkillsForSearchViewModel();


            int minLevelId = (await _userService.GetLevels().OrderBy(x => x.Order).FirstAsync()).Id; // todo to service ?

            specifyingSkillsViewModel1.LevelsViewModel = await _userService.GetLevels().OrderBy(x => x.Order)
                .Select(x => new LevelViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync(); // todo

            specifyingSkillsViewModel1.SpecifyingSkills =
                from s in _userService.Skill()
                join sk in _userService.SubSkill()

                    on s.Id equals sk.SkillId into g
                select new SpecifyingSkillForSearchViewModel()
                {
                    SkillViewModel = new SkillViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name
                    },
                    SubSkillListViewModel = g.Select(x => new SpecifyingSubSkillForSearchViewModel()
                    {
                        SubSkillViewModel = new SubSkillViewModel()
                        {
                            Id = x.Id,
                            SkillId = x.SkillId,
                            Name = x.Name
                        },
                        LevelId = minLevelId,
                        OrHigher = false

                    })
                };

            return View(specifyingSkillsViewModel1);
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
                                                   join n in _identityService.GetUsers()
                                                       on id equals n.Id
                                                   select new UserViewModel()
                                                   {
                                                       Id = n.Id,
                                                       Name = n.Name
                                                   };

                foreach (var u in Users)
                {
                    usersSearchResultViewModel.UserSearchListResultViewModel.Add(new UserSearchResultViewModel()
                    {
                        UserViewModel = new UserViewModel()
                        {
                            Id = u.Id,
                            Name = u.Name
                        },
                        SpecifyingSkills = (from s in _userService.Skill()
                                            join osk in
                                                (from sk in _userService.SubSkill()
                                                 join spk in _userService.GetSpecifyingSkills().Where(x => x.UserId == u.Id)
                                                     on sk.Id equals spk.SubSkillId
                                                 join lvl in _userService.GetLevels()
                                                     on spk.LevelId equals lvl.Id
                                                 select new SpecifyingSubSkillViewModel
                                                 {
                                                     SubSkillViewModel = new SubSkillViewModel()
                                                     {
                                                         Id = sk.Id,
                                                         Name = sk.Name,
                                                         SkillId = sk.SkillId
                                                     },
                                                     Level = new LevelViewModel()
                                                     {
                                                         Id = spk.LevelId,
                                                         Name = lvl.Name
                                                     }


                                                 })
                                                on s.Id equals osk.SubSkillViewModel.SkillId into g
                                            select new SpecifyingSkillViewModel1()
                                            {
                                                SkillViewModel = new SkillViewModel()
                                                {
                                                    Id = s.Id,
                                                    Name = s.Name
                                                },
                                                SubSkillListViewModel = g
                                            }).ToList()
                    });
                }
            }

            // save search result on client side to pass them to PrintSearch
            usersSearchResult.SpecifyingSkillsForSearchSaveModel =
                usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel;
            usersSearchResult.UserSearchListResultViewModel =
                usersSearchResultViewModel.UserSearchListResultViewModel;
            return View("SearchResult", usersSearchResultViewModel);
        }
        [Authorize(Roles = "manager")]
        public ActionResult PrintSearch(UsersSearchResult usersSearchResult)
        {
            UsersSearchResultViewModel usersSearchResultViewModel = new UsersSearchResultViewModel();
            usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel = usersSearchResult.SpecifyingSkillsForSearchSaveModel;
            usersSearchResultViewModel.UserSearchListResultViewModel = usersSearchResult.UserSearchListResultViewModel;
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
