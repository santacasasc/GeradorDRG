using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
    public class PacienteTeste
    {   [Key]
        public int Id { get; set; }

        [ForeignKey("Configuracao")]
        public int ConfiguracaoId { get; set; }

        public string CodPaciente { get; set; }

        public string NomePaciente { get; set; }

        public virtual Configuracao Configuracao { get; set; }

     
    }
}
