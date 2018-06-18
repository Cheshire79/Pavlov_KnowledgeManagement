using System;

namespace WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
        // @Html.PageLinks(Model.PagingInfo, x => Url.Action("List", new { page = x+1 }))
    }
}