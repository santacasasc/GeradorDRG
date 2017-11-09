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
    public class VerificaConexaoController : ApiConnectionStringController
    {
        // GET: GerarDados
        public string Get()
        {
            string conexao = "N";

            string queryconexao = $@"SELECT 'A' from sys.dual";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var command = new OleDbCommand();

                command = new OleDbCommand(queryconexao, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        conexao = "S";
                    }
                }

                connection.Close();
            }

            return conexao;
        }
    }
}
