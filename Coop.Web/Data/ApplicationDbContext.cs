using System;
using Coop.Domain.Advertisements;
using Coop.Domain.Articles;
using Coop.Domain.Realties;
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

        public DbSet<Article> Articles { get; set; }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Realty> Realties { get; set; }

        public DbSet<RealtyDebt> RealtyDebts { get; set; }

        public DbSet<RealtyPay> RealtyPays { get; set; }

        public DbSet<RealtyOwner> RealtyOwners { get; set; }
    }
}