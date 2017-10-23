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
			//this.Banco = new Banco();
			this.Prestadores = new List<PrestadorTeste>();
            //this.Sistema = new Sistema();
            
        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("Banco")]
        public int BancoId { get; set; }

        [ForeignKey("Sistema")]
        public int SistemaId { get; set; }

        public string BancoURL { get; set; }

        public string BancoUsuario { get; set; }

        public string BancoSenha { get; set; }

        public string BancoSID { get; set; }

        public string CodDRG { get; set; }

        public string NomeDRG { get; set; }

        public bool UtilizaWebService { get; set; }

        public string WebServiceUsuario { get; set; }

        public string WebServiceSenha { get; set; }

        public virtual Banco Banco { get; set; }

        public virtual Sistema Sistema { get; set; }

        public IList<PacienteTeste> Pacientes { get; set; }

        public IList<PrestadorTeste> Prestadores { get; set; }

    }
}
