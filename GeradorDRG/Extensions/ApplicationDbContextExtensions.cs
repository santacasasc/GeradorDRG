using GeradorDRG.Data;
using GeradorDRG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static void Seed(this ApplicationDbContext context)
        {

            context.Database.Migrate();

            IList<Banco> bancos = new List<Banco>();

            Banco bancoOracle = new Banco { Nome = "Oracle", Padrao = true, Provider = Provider.oracle };

            bancos.Add(bancoOracle);

            IList<Sistema> sistemas = new List<Sistema>();

            Sistema sistemaMv = new Sistema { Nome = "MV", Padrao = true };

            sistemas.Add(sistemaMv);

            foreach (var b in bancos)
            {
                var banco = context.Banco.Where(m => m.Nome == b.Nome && m.Padrao).FirstOrDefault();
                if (banco == null)
                {
                    context.Banco.Add(b);
                }
            }

            foreach (var s in sistemas)
            {
                var sitema = context.Sistema.Where(m => m.Nome == s.Nome && m.Padrao).FirstOrDefault();
                if (sitema == null)
                {
                    context.Sistema.Add(s);
                }
            }

            if (context.SistemaBanco.Where(m => m.Banco.Nome == "Oracle" && m.Banco.Padrao && m.Sistema.Nome == "MV" && m.Sistema.Padrao).FirstOrDefault() == null)
            {
                context.SistemaBanco.Add(new SistemaBanco
                {
                    Banco = bancoOracle,
                    Sistema = sistemaMv
                });
            }

            // Save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }

    }
}
