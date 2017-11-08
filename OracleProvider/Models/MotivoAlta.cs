using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OracleProvider.Models
{
    public class MotivoAlta
    {
        public MotivoAlta(){

        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Configuracao")]
        public int ConfiguracaoId { get; set; }

        [Required]
        public string CodigoMotivo { get; set; }

        public string DsMotivoAlta { get; set; }

        public string TipoDRG { get; set; }

    }
}
