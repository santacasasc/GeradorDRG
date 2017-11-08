using OracleProvider.Extensions;
using OracleProvider.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OracleProvider.Controllers
{
    [System.Web.Http.Authorize]
    public class TipoDeInternacaoController : ApiConnectionStringController
    {
        // GET: GerarDados
        public IList<TipoInterncao> Get()
        {
            IList<TipoInterncao> Tipos = new List<TipoInterncao>();

            string queryTipoInternacao = $@"SELECT CodigoTipo,Tipo FROM dbamv.VIEW_GERADOR_DRG_TP_INTERNACAO";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var command = new OleDbCommand();

                command = new OleDbCommand(queryTipoInternacao, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        Tipos.Add(new TipoInterncao
                        {
                            CodigoTipo = reader["CodigoTipo"].ToString()
                                ,
                            Tipo = reader["Tipo"].ToString()
                        });
                    }
                }

                connection.Close();
            }

            return Tipos;
        }
    }
}
