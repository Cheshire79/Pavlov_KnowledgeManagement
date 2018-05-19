using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using Microsoft.AspNet.Identity;
using WebUI.Models.KnowledgeManagement;

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
        public async Task<ActionResult> IndexQ()
        {
            SpecifyingSkillsViewModel_Queryable specifyingSkillsViewModel1 = new SpecifyingSkillsViewModel_Queryable();
            await Task.Run(() =>
            {
                string currentUserId = HttpContext.User.Identity.GetUserId();
                int minLevelId = _userService.GetLevels().OrderBy(x => x.Order).First().Id;

                specifyingSkillsViewModel1.LevelsViewModel = _userService.GetLevels().OrderBy(x => x.Order)
                    .Select(x => new LevelViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList(); // todo

                specifyingSkillsViewModel1.SpecifyingSkills =
                    from s in _userService.Skill()
                    join osk in
                        (from sk in _userService.SubSkill()
                         join spk in _userService.GetSpecifyingSkills().Where(x => x.UserId == currentUserId)
                             on sk.Id equals spk.SubSkillId into g
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
                                 Id =
                                     g.DefaultIfEmpty(new SpecifyingSkillDTO()
                                     {
                                         Id = 0,
                                         LevelId = minLevelId,
                                         SubSkillId = 0,
                                         UserId = ""
                                     }).FirstOrDefault().LevelId
                                 
                             }
                             // absent of  SpecifyingSkillDTO means that user has no skill.
                             // as skell "None" storage at first minimum order  at database
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
                    };

                //select * from skills
                //left join SubSkills on skills.Id=SubSkills.SkillId
                //left join  SpecifyingSkills on SpecifyingSkills.SubSkillId=SubSkills.Id 

            });
            return View(specifyingSkillsViewModel1);

        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> IndexQ(SpecifyingSkillsSaveModel specifyingSkillsViewModel)
        {
            await Task.Run(() =>
            {
                if (ModelState.IsValid)
                {
                    string currentUserId = HttpContext.User.Identity.GetUserId();
                    List<SpecifyingSkillDTO> result =
                        specifyingSkillsViewModel.SpecifyingSkills.Where(x => x.SubSkillListViewModel != null)
                            .SelectMany(x => x.SubSkillListViewModel,
                                (x, y) => new SpecifyingSkillDTO()
                                {
                                    SubSkillId = y.SubSkillViewModel.Id,
                                    LevelId = y.Level.Id //, UserId = currentUserId 
                                }).ToList();
                    _userService.SaveSpecifyingSkill(result, currentUserId);
                }
            });
            return RedirectToAction("Index", "Home");
        }
        


        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }

    }

}