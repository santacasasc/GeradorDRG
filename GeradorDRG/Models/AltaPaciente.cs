using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
    public class AltaPaciente
    {
        public AltaPaciente()
        {

        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("Configuracao")]
        public int ConfiguracaoId { get; set; }

        public string CodigoMotivo { get; set; }

        public string MotivoAlta { get; set; }

        public TipoDRG Tipo { get; set; }

        public virtual Configuracao Configuracao { get; set; }


        public enum TipoDRG 
        {
            [Display(Name = "A (Casa/Auto-Cuidado)")]A,


            [Display(Name = "I (Instituição de longa permanência)")]I,

            [Display(Name = "D (Atenção domiciliar)")]D,

            [Display(Name = "P (Alta a pedido)")]P,

            [Display(Name = "C (Transferido para hospital de curta permanência)")]C,

            [Display(Name = "L (Transferido para hospital de longa permanência (unidade de cuidados crônicos, reabilitação))")]L,

            [Display(Name = "O (Óbito)")]O,

            [Display(Name = "E (Evasão)")]E
        }
    }
}
