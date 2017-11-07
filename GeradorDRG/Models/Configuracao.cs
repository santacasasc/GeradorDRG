using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Models
{
	public class Configuracao
	{
		public Configuracao() {
			this.Pacientes=new List<PacienteTeste>();
			this.Prestadores = new List<PrestadorTeste>();
            this.MotivosAlta = new List<MotivoAlta>();
            this.MotivosAlta = new List<MotivoAlta>();
        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("Banco")]
        public int BancoId { get; set; }

        [ForeignKey("Sistema")]
        public int SistemaId { get; set; }

        //[ForeignKey("AltaPaciente")]
        [Display(Name ="URL do Banco")]
        public string BancoURL { get; set; }

        [Display(Name = "Usuário do Banco")]
        public string BancoUsuario { get; set; }

        [Display(Name = "Senha do Banco")]
        public string BancoSenha { get; set; }

        [Display(Name = "SID")]
        public string BancoSID { get; set; }

        [Display(Name = "Código DRG")]
        public string CodDRG { get; set; }

        [Display(Name = "Nome DRG")]
        public string NomeDRG { get; set; }

        public bool UtilizaWebService { get; set; }

        [Display(Name = "Usuário WebService")]
        public string WebServiceUsuario { get; set; }

        [Display(Name = "Senha WebService")]
        public string WebServiceSenha { get; set; }

        public virtual Banco Banco { get; set; }

        public virtual Sistema Sistema { get; set; }


        public IList<PacienteTeste> Pacientes { get; set; }

        public IList<PrestadorTeste> Prestadores { get; set; }

        public IList<MotivoAlta> MotivosAlta { set; get; }

        public IList<TipoInterncao> TiposInterncao { set; get; }
    }
}
