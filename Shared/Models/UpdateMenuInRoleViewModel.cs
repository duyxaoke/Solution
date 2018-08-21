﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UpdateMenuInRoleViewModel
    {
        public Guid RoleId { get; set; }
        public List<int> MenuIds { get; set; }
       
    }
}
