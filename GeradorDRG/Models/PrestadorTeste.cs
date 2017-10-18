using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
    public class PrestadorTeste
    {
        public int Id { get; set; }

        [ForeignKey("Configuracao")]
        public int ConfiguracaoId { get; set; }

        public string CodPrestador { get; set; }

        public string NomePrestador { get; set; }

        public virtual Configuracao Configuracao { get; set; }
    }
}
