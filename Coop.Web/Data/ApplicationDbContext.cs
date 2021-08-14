using System;
using System.Collections.Generic;
using System.Text;
using Coop.Domain.Advertisements;
using Coop.Domain.Articles;
using Coop.Domain.Common;
using Coop.Domain.Realties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Coop.Web.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        DbSet<Article> Articles { get; }
        
        DbSet<Advertisement> Advertisements { get; }
        
        DbSet<Realty> Realties { get; }
    }
}