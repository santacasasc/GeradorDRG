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
    public class MotivoAltaController : ApiConnectionStringController
    {
        public IList<MotivoAlta> Get()
        {
            IList<MotivoAlta> Motivos = new List<MotivoAlta>();

            string queryMotivoAlta = $@"SELECT CodigoMotivo,DsMotivoAlta FROM DBAMV.VIEW_GERADOR_DRG_MOTIVO_ALTA";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var command = new OleDbCommand();

                command = new OleDbCommand(queryMotivoAlta, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        Motivos.Add(new MotivoAlta
                        {
                            CodigoMotivo = reader["CodigoMotivo"].ToString()
                                ,
                            DsMotivoAlta = reader["DsMotivoAlta"].ToString()
                        });
                    }
                }

                connection.Close();
            }

            return Motivos;
        }
    }
}
