using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
    public class SistemaBanco
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Banco")]
        public int BancoId { get; set; }

        [ForeignKey("Sistema")]
        public int SistemaId { get; set; }

        [DefaultValue(false)]
        public bool Padrao { get; set; }

        public virtual Banco Banco { get; set; }

        public virtual Sistema Sistema { get; set; }

    }
}
