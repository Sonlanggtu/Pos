using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.WebApplication.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Yêu cầu nhập username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập password")]
        public string Password { get; set; }

    }
}
