using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DDPS.Web.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DDPSUser class
public class DDPSUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

