using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Security
{
    public class AppIdentityUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
