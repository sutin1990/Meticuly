using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class VW_MFC_UserMasters
    {
        [Key]
        public int UserId { get; set; }
        //[StringLength(60, MinimumLength = 6)]
        [Required]
        public string UserName { get; set; }
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\d\w\._\-]+@([\d\w\._\-]+\.)+[\w]+$", ErrorMessage = "Email is invalid")]
        public string UserEmail { get; set; }
        [Required]
        public string ClusterCode { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public string DepID { get; set; }
        [Required(ErrorMessage = "Shift is required")]
        public int ShiftID { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
        public string CompanyCode { get; set; }
        public string LocationCode { get; set; }
        public string UserLocationId { get; set; }
        public byte[] UserImage { get; set; }
        //[DataType(DataType.Date)]
        [Required]
        public DateTime? UserExpireDate { get; set; }
        //[DataType(DataType.Date)]
        [Required]
        public DateTime? CreateDate { get; set; }
        public string UserCreated { get; set; }
        //[DataType(DataType.Date)]
        [Required]
        public DateTime? TransDate { get; set; }
        //[DataType(DataType.Date)]
        [Required]
        public DateTime? LastSignedin { get; set; }
        public string ModifyBy { get; set; }
        public string UserRoll { get; set; }

        public string MenuName { get; set; }

    }
}
