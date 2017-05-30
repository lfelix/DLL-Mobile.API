using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DLLMobileAPI
{
    public class ApiContext : DbContext
    {
        public virtual DbSet<ApplicationUser> Users { get; set; }

        public virtual DbSet<LoginActivity> LoginActivities { get; set; }   
    }
}