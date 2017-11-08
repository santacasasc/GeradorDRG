using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OracleProvider.Extensions
{
    public class ApiConnectionStringController : ApiController
    {
        public readonly string connectionString = "Provider=OraOLEDB.Oracle.1;Data Source=producao.world;User ID=GERADOR_DRG;Password=GERADOR_DRG";
    }
}