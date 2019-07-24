using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Models
{
    public class LoginViewModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Username { get; set; }


        [StringLength(60, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
