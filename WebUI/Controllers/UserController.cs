using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using Microsoft.AspNet.Identity;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.SpecifyingSkills;
using WebUI.Models.SpecifyingSkills.SaveViewModel;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService managerService)
        {
            _userService = managerService;
        }

        [Authorize(Roles = "user")]
        public async Task<ActionResult> Index()
        {
            SpecifyingSkillsViewModel specifyingSkillsViewModel = new SpecifyingSkillsViewModel();
           
                string currentUserId = HttpContext.User.Identity.GetUserId();
                int minLevelId = await _userService.GetIdForMinLevelValue();

                specifyingSkillsViewModel.Levels = await _userService.GetLevels().OrderBy(x => x.Order)
                    .Select(x => new LevelViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync(); 

                specifyingSkillsViewModel.SpecifyingSkills =
                   await (from skill in _userService.Skill()
                    join specifyingSkillViewModel in
                        (from subSkill in _userService.SubSkill()
                         join specifyingSkill in _userService.GetSpecifyingSkills().Where(x => x.UserId == currentUserId)
                             on subSkill.Id equals specifyingSkill.SubSkillId into specifyingSkillsDTO
                         select new SpecifyingSubSkillViewModel
                         {
                             SubSkill = new SubSkillViewModel()
                             {
                                 Id = subSkill.Id,
                                 Name = subSkill.Name,
                                 SkillId = subSkill.SkillId
                             },
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
                         })
                        on skill.Id equals specifyingSkillViewModel.SubSkill.SkillId into specifyingSubSkillViewModel
                    select new SpecifyingSkillViewModel()
                    {
                        Skill = new SkillViewModel()
                        {
                            Id = skill.Id,
                            Name = skill.Name
                        },
                        SpecifyingSubSkills = specifyingSubSkillViewModel.ToList()
                    }).ToListAsync();

                //select * from skills
                //left join SubSkills on skills.Id=SubSkills.SkillId
                //left join  SpecifyingSkills on SpecifyingSkills.SubSkillId=SubSkills.Id 

            return View(specifyingSkillsViewModel);

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