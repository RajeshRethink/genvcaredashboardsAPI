using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace genvcaredashboardsAPI.Models.General
{
    public class SecurityDto
    {
        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }
    }
}
