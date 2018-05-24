using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Services;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Controllers
{
    public class ManagerSkillController : Controller
    {
        //
        // GET: /Manager/

        //  private IManagerService _managerService;
        private ISubSkillService _subSkillService;
        private ISkillService _skillService;

        public ManagerSkillController(ISubSkillService subSkillService, ISkillService skillService)
        {
            _subSkillService = subSkillService;
            _skillService = skillService;

        }


        #region Skill
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Skills(string message)
        {
            TempData["message"] = message;
            IEnumerable<SkillViewModel> viewModel =
                await Task.Run(() =>

                _skillService.GetAll().OrderBy(x => x.Name).Select(x => new SkillViewModel
                {
                    Name = x.Name,
                    Id = x.Id
                }));

            return View(viewModel);
        }

        [Authorize(Roles = "manager")]
        public ActionResult CreateSkill(string returnUrl)
        {
            SkillViewModel skillViewModel =


                   new SkillViewModel()
                   {
                       ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl
                   };

            return View(skillViewModel);

        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> CreateSkill(SkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() =>
               _skillService.Create(new SkillDTO() { Name = model.Name }));
            }
            return RedirectToAction("Skills"); // todo to last pages
        }


        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> DeleteSkill(int id, string returnUrl)
        {

            try
            {
                await Task.Run(() =>
                 _skillService.Delete(id)
                 );
                string temp = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl;
                return Redirect(temp);
            }

            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages
            }

            // todo to last pages
        }

        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSkill(int? id, string returnUrl)
        {
            if (id == null)
                return HttpNotFound("Missed id value");
            try
            {
                SkillViewModel skillViewModel =
               await Task.Run(() =>
               {
                   var skill = _skillService.Get(id.Value);
                   return new SkillViewModel()
                   {
                       Id = skill.Id,
                       Name = skill.Name,
                       ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl
                   };
               });
                return View(skillViewModel);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages

            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSkill(SkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() =>
                        _skillService.Update(new SkillDTO() { Id = model.Id, Name = model.Name }));
                }

                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                    //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages
                }
            }
            return RedirectToAction("Skills"); // todo to last pages
        }

        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSubSkills(int? skillId, string returnUrl)
        {

            if (skillId == null)
                return HttpNotFound("Missed id value");
            try
            {
                SubSkillListViewModel subSkillListViewModel =
               await Task.Run(() =>
                       new SubSkillListViewModel()
                       {
                           SubSkillsViewModel = _subSkillService.GetSubSkillBySkillId(skillId.Value)
                               .Select(x => new SubSkillViewModel() { Id = x.Id, Name = x.Name, SkillId = x.SkillId }),
                           SkillViewModel = new SkillViewModel()
                           {
                               Id = skillId.Value,
                               Name = _skillService.Get(skillId.Value).Name
                           },
                           ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl
                       });
                return View(subSkillListViewModel);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                // todo to last pages

            }
        }

        #endregion

        #region SubSkill
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> CreateSubSkill(int? skillId, string returnUrl)
        {
            if (skillId == null)
                return HttpNotFound("Missed id value");
            try
            {
                SubSkillViewModel subSkillViewModel =
               await Task.Run(() =>
               {
                   var skill = _skillService.Get(skillId.Value);
                   return new SubSkillViewModel()
                   {
                       SkillId = skill.Id,                       
                       ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("EditSubSkills", new { skillId }) : returnUrl
                   };
               });
                return View(subSkillViewModel);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                // todo to last pages
            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> CreateSubSkill(SubSkillViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() =>
                        _subSkillService.Create(new SubSkillDTO() { SkillId = model.SkillId, Name = model.Name }));
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                    //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages

                }
            }
            return RedirectToAction("EditSubSkills", new { model.SkillId }); // todo to last pages


        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> DeleteSubSkill(int subSkillid, string returnUrl)
        {
            try
            {
                await Task.Run(() =>
                 _subSkillService.Delete(subSkillid));
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skill") : returnUrl);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                // todo to last pages
            }
            // todo to last pages
        }

        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSubSkill(int? subSkillId, string returnUrl)
        {
            if (subSkillId == null)
                return HttpNotFound("Missed id value");
            try
            {
                SubSkillViewModel subSkillViewModel =
               await Task.Run(() =>
               {
                   var subSkill = _subSkillService.Get(subSkillId.Value);

                   return new SubSkillViewModel()
                   {
                       Id = subSkill.Id,
                       SkillId = subSkill.SkillId,
                       Name = subSkill.Name,
                       ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("EditSubSkills", new { subSkill.SkillId }) : returnUrl                       
                   };
               });
                return View(subSkillViewModel);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
                //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages

            }
        }


        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSubSkill(SubSkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() =>
                        _subSkillService.Update(new SubSkillDTO() { Id = model.Id, Name = model.Name, SkillId = model.SkillId }));
                }

                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                    //   return RedirectToAction("Skills", new {ex.Message}); // todo to last pages
                }
            }
            return Redirect(model.ReturnUrl); // todo to last pages
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            _subSkillService.Dispose(); ;
            _skillService.Dispose();

            base.Dispose(disposing);
        }
    }
}