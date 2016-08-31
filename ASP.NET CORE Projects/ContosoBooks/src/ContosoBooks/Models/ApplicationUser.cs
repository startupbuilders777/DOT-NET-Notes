using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ContosoBooks.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    /*
     Add Entity Framework
Open the project.json file. In the dependencies section, add the following line:
"dependencies": {
  ...
  "EntityFramework.SqlServer": "7.0.0-beta8"
},
         */
    public class ApplicationUser : IdentityUser
    {
    }
}
