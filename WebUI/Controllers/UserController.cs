using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Interface;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using Microsoft.AspNet.Identity;
using WebUI.Mapper;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.SpecifyingSkills;
using WebUI.Models.SpecifyingSkills.SaveViewModel;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserService<SkillDTO, SubSkillDTO, SpecifyingSkillDTO, LevelDTO, SpecifyingSkillForSearchDTO> _userService;
        private IMapper _mapper;
        public UserController(IUserService<SkillDTO, SubSkillDTO, SpecifyingSkillDTO, LevelDTO, SpecifyingSkillForSearchDTO> managerService, IMapperFactoryWEB mapperFactory)
        {
            _userService = managerService;
            _mapper = mapperFactory.CreateMapperWEB();
        }

        [Authorize(Roles = "user")]
        public async Task<ActionResult> Index()
        {
            SpecifyingSkillsViewModel specifyingSkillsViewModel = new SpecifyingSkillsViewModel();

            string currentUserId = HttpContext.User.Identity.GetUserId();
            int minLevelId = await _userService.GetIdForMinLevelValue();

            specifyingSkillsViewModel.Levels = _mapper.Map<IEnumerable<LevelDTO>, IEnumerable<LevelViewModel>>
                (await _userService.GetLevels().OrderBy(x => x.Order).ToListAsync());

            specifyingSkillsViewModel.SpecifyingSkills =
                   await (from skill in _userService.Skill().ProjectTo<SkillViewModel>(_mapper.ConfigurationProvider)
                          join specifyingSkillViewModel in GetSpecifyingSubSkillsForUser(currentUserId, minLevelId)
                        on skill.Id equals specifyingSkillViewModel.SubSkill.SkillId into specifyingSubSkillViewModel
                          select new SpecifyingSkillViewModel()
                          {
                              Skill = skill,
                              SpecifyingSubSkills = specifyingSubSkillViewModel.ToList()
                          }).ToListAsync();
            //select * from skills
            //left join SubSkills on skills.Id=SubSkills.SkillId
            //left join  SpecifyingSkills on SpecifyingSkills.SubSkillId=SubSkills.Id 
            return View(specifyingSkillsViewModel);
        }

        private IQueryable<SpecifyingSubSkillViewModel> GetSpecifyingSubSkillsForUser(string iserId, int minLevelId)
        {
            var formForSpecifyingSubSkills =
                (from subSkill in _userService.SubSkill().ProjectTo<SubSkillViewModel>(_mapper.ConfigurationProvider)
                 join specifyingSkill in _userService.GetSpecifyingSkills().Where(x => x.UserId == iserId)
                     on subSkill.Id equals specifyingSkill.SubSkillId into specifyingSkillsDTO
                 select new SpecifyingSubSkillViewModel
                 {
                     SubSkill = subSkill,
                     LevelId =
                         specifyingSkillsDTO.DefaultIfEmpty(new SpecifyingSkillDTO()
                         {
                             Id = 0,
                             LevelId = minLevelId,
                             SubSkillId = 0,
                             UserId = ""
                         }).FirstOrDefault().LevelId
                     // absent of  SpecifyingSkillDTO means that user has no skill.
                     // as skell "None" storage at first minimum order  at database
                 });
            return formForSpecifyingSubSkills;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        //  public async Task<ActionResult> Index(SpecifyingSkillsViewModel specifyingSkillsViewModel
        public async Task<ActionResult> Index(SpecifyingSkillsSaveViewModel specifyingSkillsSave)
        {
            // two way 
            // use the same type SpecifyingSkillsViewModel
            // or special type to save data SpecifyingSkillsSaveModel - which one is better ?

            #region using SpecifyingSkillsViewModel to save changes
            /*
            if (ModelState.IsValid)
                {
                    string currentUserId = HttpContext.User.Identity.GetUserId();
                    List<SpecifyingSkillDTO> result =
                        specifyingSkillsViewModel.SpecifyingSkills.Where(x => x.SpecifyingSubSkills != null)
                            .SelectMany(x => x.SpecifyingSubSkills,
                                (x, y) => new SpecifyingSkillDTO()
                                {
                                    SubSkillId = y.SubSkill.Id,
                                    LevelId = y.LevelId 
                                }).ToList();
                   await _userService.SaveSpecifyingSkill(result, currentUserId);
                }*/
            #endregion
            if (ModelState.IsValid)
            {
                string currentUserId = HttpContext.User.Identity.GetUserId();
                List<SpecifyingSkillDTO> result =
                    specifyingSkillsSave.SpecifyingSkills.Where(x => x.SubSkills != null)
                        .SelectMany(x => x.SubSkills,
                            (x, y) => new SpecifyingSkillDTO()
                            {
                                SubSkillId = y.SubSkillId,
                                LevelId = y.LevelId
                            }).ToList();
                await _userService.SaveSpecifyingSkill(result, currentUserId);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }

    }

}