using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
    public class Sistema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [DefaultValue(false)]
        public bool Padrao { get; set; }

        public virtual IList<SistemaBanco> SistemaBanco { get; set; }
        
    }

}
