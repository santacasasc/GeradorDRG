using GeradorDRG.Data;
using GeradorDRG.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static void Seed(this ApplicationDbContext context)
        {
            // Perform database delete and create
            //context.Database.EnsureDeleted();
            context.Database.Migrate();

            IList<Banco> bancos = new List<Banco>();

            bancos.Add(new Banco { Nome = "Oracle", Padrao = true, Provider = Provider.oracle });
            bancos.Add(new Banco { Nome = "Skt", Padrao = true, Provider = Provider.skt });

            IList<Sistema> sistemas = new List<Sistema>();

            sistemas.Add(new Sistema { Nome = "MV", Padrao = true});
            sistemas.Add(new Sistema { Nome = "Telecom", Padrao = true });


            foreach (var b in bancos)
            {
                var banco = context.Banco.Where(m => m.Nome == b.Nome).FirstOrDefault();
                if (banco == null)
                {
                    context.Banco.Add(b);
                }
            }

            foreach (var s in sistemas)
            {
                var sitema = context.Sistema.Where(m => m.Nome == s.Nome).FirstOrDefault();
                if (sitema == null)
                {
                    context.Sistema.Add(s);
                }
            }

            // Save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }


    }
}
