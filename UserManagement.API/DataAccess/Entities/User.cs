using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UserManagement.API.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public string Website { get; set; }
        public int? CompanyId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Company Company { get; set; }
    }
}
