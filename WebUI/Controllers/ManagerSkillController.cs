using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Services;
using WebUI.Mapper;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Controllers
{
    public class ManagerSkillController : Controller
    {
        private ISubSkillService _subSkillService;
        private ISkillService _skillService;
        private IMapper _mapper;

        public ManagerSkillController(ISubSkillService subSkillService, ISkillService skillService, IMapperFactoryWEB mapperFactory)
        {
            _subSkillService = subSkillService;
            _skillService = skillService;
            _mapper = mapperFactory.CreateMapperWEB();
        }

        #region Skill
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Skills(string message)
        {
            TempData["message"] = message;
            IEnumerable<SkillViewModel> viewModel = _mapper.Map<IEnumerable<SkillDTO>, IEnumerable<SkillViewModel>>
            (await _skillService.GetAll().OrderBy(x => x.Name).ToListAsync());
            return View(viewModel);
        }

        [Authorize(Roles = "manager")]
        public ActionResult CreateSkill(string returnUrl)
        {
            SkillViewModel skillViewModel = new SkillViewModel()
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
                var skillDTO = _mapper.Map<SkillViewModel, SkillDTO>(model);
                skillDTO.Id = new SkillDTO().Id;
                await _skillService.Create(skillDTO);
            }
            return RedirectToAction("Skills"); // todo  returnUrl
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> DeleteSkill(int id, string returnUrl)
        {
            try
            {
                await _skillService.Delete(id);
                string temp = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl;
                return Redirect(temp);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            //   return RedirectToAction("Skills", new {ex.Message}); // todo  returnUrl
        }

        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSkill(int? id, string returnUrl)
        {
            if (id == null)
                return HttpNotFound("Missed id value");
            try
            {
                SkillDTO skillDTO = await _skillService.GetByIdAsync(id.Value);
                var skillViewModel = _mapper.Map<SkillDTO,SkillViewModel>(skillDTO);
                skillViewModel.ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl;
                return View(skillViewModel);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            //   return RedirectToAction("Skills", new {ex.Message}); // todo  returnUrl
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSkill(SkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _skillService.Update(_mapper.Map<SkillViewModel, SkillDTO>(model));
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }
            }
            return RedirectToAction("Skills"); // todo  returnUrl
        }

        [Authorize(Roles = "manager")]
        public async Task<ActionResult> EditSubSkills(int? skillId, string returnUrl)
        {
            if (skillId == null)
                return HttpNotFound("Missed id value");
            try
            {
                SubSkillsViewModel subSkillListViewModel =
                       new SubSkillsViewModel()
                       {
                           SubSkills = (await _subSkillService.GetSubSkillBySkillId(skillId.Value)).
                               ProjectTo<SubSkillViewModel>(_mapper.ConfigurationProvider),
                           Skill = new SkillViewModel()
                           {
                               Id = skillId.Value,
                               Name = (await _skillService.GetByIdAsync(skillId.Value)).Name
                           },
                           ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl
                       };
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
                var skill = await _skillService.GetByIdAsync(skillId.Value);
                SubSkillViewModel subSkillViewModel =
                    new SubSkillViewModel()
                    {
                        SkillId = skill.Id,
                        ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("EditSubSkills", new { skillId }) : returnUrl
                    };
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
                    await _subSkillService.Create( _mapper.Map<SubSkillViewModel, SubSkillDTO>(model));
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
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
                await _subSkillService.Delete(subSkillid);
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Skills") : returnUrl);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
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
                SubSkillDTO subSkillDTO = await _subSkillService.GetByIdAsync(subSkillId.Value);
              
                SubSkillViewModel subSkillViewModel = _mapper.Map<SubSkillDTO, SubSkillViewModel>(subSkillDTO);
                subSkillViewModel.ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("EditSubSkills", new { subSkillDTO.SkillId }) : returnUrl;
                //todo check!!!!
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
                    await _subSkillService.Update(_mapper.Map<SubSkillViewModel,SubSkillDTO>(model));
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
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