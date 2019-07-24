using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class UserMaster
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [StringLength(60, MinimumLength = 6)]
        [Required]
        public string UserName { get; set; }

        [StringLength(60, MinimumLength = 6)]
        [Required]
        public string UserPassword { get; set; }

        public string UserEmail { get; set; }
        public string ClusterId { get; set; }
        public string CompanyId { get; set; }
        public string LocationId { get; set; }
        public string UserLayer { get; set; }
        public byte[] UserImage { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? UserExpireDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateCreated { get; set; }
        public string UserCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateChanged { get; set; }
        public string UserChanged { get; set; }
        public string UserLocationId { get; set; }
        public string PrintBillFlag { get; set; }
        public int? MaxUserQty { get; set; }
        public String MaxAddress { get; set; }
    }
}
