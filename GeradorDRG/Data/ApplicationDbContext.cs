using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GeradorDRG.Models;

namespace GeradorDRG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<GeradorDRG.Models.Banco> Banco { get; set; }

        public DbSet<GeradorDRG.Models.Sistema> Sistema { get; set; }

        public DbSet<GeradorDRG.Models.SistemaBanco> SistemaBanco { get; set; }

        public DbSet<GeradorDRG.Models.PacienteTeste> PacienteTeste { get; set; }

        public DbSet<GeradorDRG.Models.PrestadorTeste> PrestadorTeste { get; set; }

        public DbSet<GeradorDRG.Models.Configuracao> Configuracao { get; set; }

        public DbSet<GeradorDRG.Models.AltaPaciente> AltaPaciente { get; set; }

    }
}