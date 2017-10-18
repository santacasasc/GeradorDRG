using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeradorDRG.Models
{
	[XmlRootAttribute("loteInternacao")]
	public class LoteInternacao
    {
		public LoteInternacao()
		{
			
			this.Internacoes = new List<Internacao>();
		}

		[XmlElement(ElementName = "Internacao")]
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
			[XmlElement(ElementName ="situacao")]
			public string Situacao { get; set; }

			[XmlElement(ElementName = "caraterInternacao")]
			public string CaraterInternacao { get; set; }

			[XmlElement(ElementName = "numeroAtendimento")]
			public string NumeroAtendimento { get; set; }

			[XmlElement(ElementName = "numeroAutorizacao")]
			public string NumeroAutorizacao { get; set; }

			[XmlElement(ElementName = "dataInternacao")]
			public DateTime DataInternacao { get; set; }

			[XmlElement(ElementName = "dataAlta")]
			public DateTime DataAlta { get; set; }

			[XmlElement(ElementName = "condicaoAlta")]
			public char CondicaoAlta { get; set; }

			[XmlElement(ElementName = "dataAutorizacao")]
			public DateTime DataAutorizacao { get; set; }

			[XmlElement(ElementName = "codigoCidPrincipal")]
			public int CodigoCidPrincipal { get; set; }

			[XmlElement(ElementName = "internadoOutrasVezes")]
			public char InternadoOutrasVezes { get; set; }

			[XmlElement(ElementName = "reinternacao")]
			public char Reinternacao { get; set; }

			[XmlElement(ElementName = "recaida")]
			public char Recaida { get; set; }




			[XmlElement(ElementName = "Hospital")]
			public Hospital Hospitais { get; set; }
			[XmlElement(ElementName = "Beneficiario")]
			public Beneficiario Beneficiarios { get; set; }
			[XmlElement(ElementName = "Operadora")]
			public Operadora Operadoras { get; set; }
			[XmlElement(ElementName = "Medico")]
			public List<Medico> Medicos { get; set; }
			[XmlElement(ElementName = "CidSecundario")]
			public List<CidSecundario> CidSecundarios { get; set; }
			[XmlElement(ElementName = "Procedimento")]
			public List<Procedimento> Procedimentos { get; set; }
			[XmlElement(ElementName = "Cti")]
			public Cti Ctis { get; set; }
			[XmlElement(ElementName = "SuporteVentilatorio")]
			public SuporteVentilatorio SuporteVentilatorios { get; set; }
			[XmlElement(ElementName = "CondicaoAdquirida")]
			public CondicaoAdquirida CondicoesAdquirida { get; set; }
			[XmlElement(ElementName = "AltaAdministrativa")]
			public AltaAdministrativa AltasAdministrativa { get; set; }
			[XmlElement(ElementName = "Rn")]
			public Rn Rns { get; set; }
			[XmlElement(ElementName = "AnaliseCritica")]
			public AnaliseCritica AnalisesCritica { get; set; }




			public class Beneficiario
			{
				[XmlElement(ElementName = "codigo")]
				public int Codigo { get; set; }
				[XmlElement(ElementName = "nome")]
				public string Nome { get; set; }
				[XmlElement(ElementName = "dataNascimento")]
				public DateTime DataNascimento { get; set; }
				[XmlElement(ElementName = "sexo")]
				public char Sexo { get; set; }
				[XmlElement(ElementName = "nomeMae")]
				public string NomeMae { get; set; }
				[XmlElement(ElementName = "cpf")]
				public int Cpf { get; set; }
				[XmlElement(ElementName = "endereco")]
				public string Endereco { get; set; }
				[XmlElement(ElementName = "recemNascido")]
				public string RecemNascido { get; set; }
			}

			public class Operadora
			{
				[XmlElement(ElementName = "codigo")]
				public int Codigo { get; set; }
				[XmlElement(ElementName = "nome")]
				public string Nome { get; set; }
				[XmlElement(ElementName = "sigla")]
				public string Sigla { get; set; }
				[XmlElement(ElementName = "plano")]
				public string Plano { get; set; }
				[XmlElement(ElementName = "numeroCarteira")]
				public int NumeroCarteira { get; set; }
				[XmlElement(ElementName = "dataValidade")]
				public DateTime DataValidade { get; set; }

			}

			public class Medico
			{
				[XmlElement(ElementName = "nome")]
				public string Nome { get; set; }
				[XmlElement(ElementName = "ddd")]
				public int Ddd { get; set; }
				[XmlElement(ElementName = "telefone")]
				public int Telefone { get; set; }
				[XmlElement(ElementName = "email")]
				public string Email { get; set; }
				[XmlElement(ElementName = "uf")]
				public string Uf { get; set; }
				[XmlElement(ElementName = "crm")]
				public string Crm { get; set; }
				[XmlElement(ElementName = "especilidade")]
				public string Especialiade { get; set; }
				[XmlElement(ElementName = "medicoResponsavel")]
				public char MedicoResponsavel { get; set; }
				[XmlElement(ElementName = "tipoAtuacao")]
				public char TipoAtuacao { get; set; }

			}

			public class Hospital
			{
				[XmlElement(ElementName = "codigo")]
				public int Codigo { get; set; }
				[XmlElement(ElementName = "nome")]
				public string Nome { get; set; }

			}

			public class CidSecundario
			{
				[XmlElement(ElementName = "codigoCidSecundario")]
				public string CodigoCidSecundario { get; set; }
			}

			public class Procedimento
			{
				[XmlElement(ElementName = "codigoProcedimento")]
				public int CodigoProcedimento { get; set; }
				[XmlElement(ElementName = "dataAutorizacao")]
				public DateTime DataAutorizacao { get; set; }
				[XmlElement(ElementName = "dataExecucao")]
				public DateTime DataExecucao { get; set; }

			}

			public class Cti
			{
				[XmlElement(ElementName = "dataInicial")]
				public DateTime DataInicial { get; set; }
				[XmlElement(ElementName = "dataFinal")]
				public DateTime DataFinal { get; set; }
				[XmlElement(ElementName = "codigoCidPrincipal")]
				public int CodigoCidPrincipal { get; set; }
				[XmlElement(ElementName = "condicaoAlta")]
				public char CondicaoAlta { get; set; }
				[XmlElement(ElementName = "uf")]
				public string Uf { get; set; }
				[XmlElement(ElementName = "crm")]
				public string Crm { get; set; }
				[XmlElement(ElementName = "codigoHospital")]
				public int CodigoHospital { get; set; }
				[XmlElement(ElementName = "nomeHospital")]
				public string NomeHospital { get; set; }



			}

			public class SuporteVentilatorio
			{
				[XmlElement(ElementName = "tipo")]
				public char Tipo { get; set; }
				[XmlElement(ElementName = "tipoInvasivo")]
				public char TipoInvasivo { get; set; }
				[XmlElement(ElementName = "local")]
				public char Local { get; set; }
				[XmlElement(ElementName = "dataInicial")]
				public DateTime DataInical { get; set; }
				[XmlElement(ElementName = "dataFinal")]
				public DateTime DataFinal { get; set; }

				public CondicaoAdquirida CondicoesAdquirida { get; set; }
			}
			public class CondicaoAdquirida
			{
				[XmlElement(ElementName = "codigoCondicaoAdquirida")]
				public int CodigoCondicaoAdquirida { get; set; }
				[XmlElement(ElementName = "dataOcorrencia")]
				public DateTime DataOcorrencia { get; set; }
			}

			public class AltaAdministrativa
			{
				[XmlElement(ElementName = "numeroAtendimento")]
				public int NumeroAtendimento { get; set; }
				[XmlElement(ElementName = "numeroAutorizacao")]
				public int NumeroAutorizacao { get; set; }
			}

			public class Rn
			{
				[XmlElement(ElementName = "pesoNascimento")]
				public int PesoNascimento { get; set; }
				[XmlElement(ElementName = "idadeGestacional")]
				public int IdadeGestacional { get; set; }
				[XmlElement(ElementName = "comprimento")]
				public int Comprimento { get; set; }


			}

			public class AnaliseCritica
			{
				[XmlElement(ElementName = "dataAnalise")]
				public DateTime DataAnalise { get; set; }
				[XmlElement(ElementName = "analiseCritica")]
				public string AnaliseCriticas { get; set; }

			}
		}
	}
}

