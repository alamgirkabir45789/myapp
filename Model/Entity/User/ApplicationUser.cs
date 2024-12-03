using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Model.Entity.DailyTask;
using myportfolio.Model.Entity.Catalogs;
using System.ComponentModel;

namespace myportfolio.Model.Entity.User
{
    public class ApplicationUser: IdentityUser
    {
        internal readonly object Roles;

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactNo { get; set; }


        [Required]
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [DefaultValue(true)]
        public bool isActive { get; set; }

        //public virtual DailyTask.DailyTask DailyTasks { get; set; }
        //public virtual LeaveRegister.LeaveRegister LeaveRegister { get; set; }
    }
}
