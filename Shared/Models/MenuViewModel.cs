using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Core.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Shared.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Guid? RoleId { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Checked { get; set; } // in role - checked
        public string RoleName { get; set; } // in role - checked
        [NotMapped]
        public List<SelectListItem> Parent { get; set; }
        [NotMapped]
        public IEnumerable<MenuViewModel> Children { get; set; }

    }
}
