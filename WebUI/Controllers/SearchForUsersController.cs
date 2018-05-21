using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Identity.Services.Interfaces;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using Rotativa.MVC;
using WebUI.Models;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.SearchForUsers;

namespace WebUI.Controllers
{
    public class SearchForUsersController : Controller
    {
        private IUserService _userService;
        private IIdentityService _identityService;
        //
        // GET: /SearchForUsers/
        public SearchForUsersController(IUserService userService, IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }
         [Authorize(Roles = "manager")]
        public async Task<ActionResult> Index()
        {
            SpecifyingSkillsForSearchViewModel specifyingSkillsViewModel1 = new SpecifyingSkillsForSearchViewModel();
            await Task.Run(() =>
            {

                int minLevelId = _userService.GetLevels().OrderBy(x => x.Order).First().Id;

                specifyingSkillsViewModel1.LevelsViewModel = _userService.GetLevels().OrderBy(x => x.Order)
                    .Select(x => new LevelViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList(); // todo

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
            });
            return View(specifyingSkillsViewModel1);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Index(List<SpecifyingSkillForSearchSaveModel> specifyingSkillsForSearchSaveModel, UsersSearchResult usersSearchResult)
        {


            UsersSearchResultViewModel usersSearchResultViewModel = new UsersSearchResultViewModel();
            await Task.Run(() =>
            {
                int minLevelId = _userService.GetLevels().OrderBy(x => x.Order).First().Id;
                int minLevelOrder = _userService.GetLevels().OrderBy(x => x.Order).First().Order;

                // clear search criteria
                specifyingSkillsForSearchSaveModel.RemoveAll(x => x.LevelId == minLevelId && !x.OrHigher);

                usersSearchResultViewModel.SpecifyingSkillsForSearchSaveModel = specifyingSkillsForSearchSaveModel;
              

                usersSearchResultViewModel.UserSearchListResultViewModel = new List<UserSearchResultViewModel>();

                if (ModelState.IsValid)
                {
                    var needSubSkill = (from needed in specifyingSkillsForSearchSaveModel
                                        join levelOrder in _userService.GetLevels()
                            on needed.LevelId equals levelOrder.Id
                                        select new { needed.SubSkillId, needed.OrHigher, levelOrder.Order }).ToList();
                    // to include in result search users who has skill level none,
                    //and criteria level marked  as none and OrHigher
                    needSubSkill.RemoveAll(x => x.OrHigher && x.Order == minLevelOrder);



                    var existedSubSkill = (from existed in _userService.GetSpecifyingSkills()
                                           join levelOrder in _userService.GetLevels()
                          on existed.LevelId equals levelOrder.Id

                                           select new { existed.SubSkillId, existed.UserId, levelOrder.Order }).ToList();

                    var users = (from specifying in existedSubSkill

                                 join needed in needSubSkill
                                     on specifying.SubSkillId equals needed.SubSkillId
                                 where ((!needed.OrHigher && needed.Order == specifying.Order)
                                        || (needed.OrHigher && needed.Order <= specifying.Order))
                                 group specifying by specifying.UserId into gr
                                 where gr.Count() == needSubSkill.Count()
                                 select new
                                 {
                                     UserId = gr.Key,

                                 }).ToList();

                    var fUsers = from u in users
                                 join n in _identityService.GetUsers()
                                     on u.UserId equals n.Id
                                 select new
                                 {
                                     UserId = n.Id,
                                     Name = n.Name
                                 };

                    usersSearchResultViewModel.UserSearchListResultViewModel = new List<UserSearchResultViewModel>();

                    foreach (var u in fUsers)
                    {
                        usersSearchResultViewModel.UserSearchListResultViewModel.Add(new UserSearchResultViewModel()
                        {
                            UserViewModel = new UserViewModel()
                                {
                                    Id = u.UserId,
                                    Name = u.Name
                                },
                            SpecifyingSkills = (from s in _userService.Skill()
                                                join osk in
                                                    (from sk in _userService.SubSkill()
                                                     join spk in _userService.GetSpecifyingSkills().Where(x => x.UserId == u.UserId)
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
                                                             Id =spk.LevelId,
                                                             Name =lvl.Name
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
            });
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
            var report = new ViewAsPdf("SearchResult", usersSearchResultViewModel);
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
