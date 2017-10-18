using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OracleProvider.Models
{
	public class LoteInternacao
    {
		public LoteInternacao()
		{
			
			this.Internacoes = new List<Internacao>();
		}

		public virtual List<Internacao> Internacoes { get; set; }

		public  class Internacao
		{
			public Internacao()
			{
				this.Medicos = new List<Medico>();

				this.Operadoras = new Operadora();

				this.Beneficiarios = new Beneficiario();

				this.Hospitais = new Hospital();

				this.CidSecundarios = new List<CidSecundario>();

				this.Procedimentos = new List<Procedimento>();

				this.Ctis = new Cti();

				this.SuporteVentilatorios = new SuporteVentilatorio();

				this.CondicoesAdquirida = new CondicaoAdquirida();

				this.AltasAdministrativa = new AltaAdministrativa();

				this.Rns = new Rn();

				this.AnalisesCritica = new AnaliseCritica();

			}
			public string Situacao { get; set; }

			public string CaraterInternacao { get; set; }

			public string NumeroAtendimento { get; set; }

			public string NumeroAutorizacao { get; set; }

			public string DataInternacao { get; set; }

			public string DataAlta { get; set; }

			public char CondicaoAlta { get; set; }

			public string DataAutorizacao { get; set; }

			public int CodigoCidPrincipal { get; set; }

			public char InternadoOutrasVezes { get; set; }

			public char Reinternacao { get; set; }

			public char Recaida { get; set; }

			public Hospital Hospitais { get; set; }

            public Beneficiario Beneficiarios { get; set; }

            public Operadora Operadoras { get; set; }

			public List<Medico> Medicos { get; set; }
			
			public List<CidSecundario> CidSecundarios { get; set; }
			
			public List<Procedimento> Procedimentos { get; set; }
			
			public Cti Ctis { get; set; }
			
			public SuporteVentilatorio SuporteVentilatorios { get; set; }
			
			public CondicaoAdquirida CondicoesAdquirida { get; set; }
			
			public AltaAdministrativa AltasAdministrativa { get; set; }
			
			public Rn Rns { get; set; }
			
			public AnaliseCritica AnalisesCritica { get; set; }

			public class Beneficiario
			{
			
				public string Codigo { get; set; }
				
				public string Nome { get; set; }
				
				public string DataNascimento { get; set; }
				
				public char Sexo { get; set; }
				
				public string NomeMae { get; set; }
				
				public string Cpf { get; set; }
				
				public string Endereco { get; set; }
				
				public string RecemNascido { get; set; }
			}

			public class Operadora
			{
				public string Codigo { get; set; }
				public string Nome { get; set; }
				public string Sigla { get; set; }
				public string Plano { get; set; }
				public string NumeroCarteira { get; set; }
				public string DataValidade { get; set; }

			}

			public class Medico
			{
				public string Nome { get; set; }
				public string Ddd { get; set; }
				public string Telefone { get; set; }
				public string Email { get; set; }
				public string Uf { get; set; }
				public string Crm { get; set; }
				public string Especialiade { get; set; }
				public char MedicoResponsavel { get; set; }
				public char TipoAtuacao { get; set; }

			}

			public class Hospital
			{
				public string Codigo { get; set; }
				public string Nome { get; set; }

			}

			public class CidSecundario
			{
				public string CodigoCidSecundario { get; set; }
			}

			public class Procedimento
			{
				public int CodigoProcedimento { get; set; }
				public DateTime DataAutorizacao { get; set; }
				public DateTime DataExecucao { get; set; }

			}

			public class Cti
			{
				public DateTime DataInicial { get; set; }

                public DateTime DataFinal { get; set; }

                public int CodigoCidPrincipal { get; set; }

                public char CondicaoAlta { get; set; }

                public string Uf { get; set; }

                public string Crm { get; set; }

                public int CodigoHospital { get; set; }

                public string NomeHospital { get; set; }

			}

			public class SuporteVentilatorio
			{
				public char Tipo { get; set; }

				public char TipoInvasivo { get; set; }

				public char Local { get; set; }

				public DateTime DataInical { get; set; }

				public DateTime DataFinal { get; set; }

				public CondicaoAdquirida CondicoesAdquirida { get; set; }
			}
			public class CondicaoAdquirida
			{
				public string CodigoCondicaoAdquirida { get; set; }
				
				public DateTime DataOcorrencia { get; set; }
			}

			public class AltaAdministrativa
			{
				public string NumeroAtendimento { get; set; }

                public string NumeroAutorizacao { get; set; }
			}

			public class Rn
			{
				
				public string PesoNascimento { get; set; }
				
				public string IdadeGestacional { get; set; }
				
				public string Comprimento { get; set; }


			}

			public class AnaliseCritica
			{
				
				public string DataAnalise { get; set; }
				
				public string AnaliseCriticas { get; set; }

			}
		}
	}
}

