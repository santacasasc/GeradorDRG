using OracleProvider.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OracleProvider.Controllers
{

    [System.Web.Http.Authorize]
    public class GerarDadosController : ApiController
    {
        // GET: GerarDados
        public LoteInternacao Get([FromUri] DateTime DataInicio, [FromUri] DateTime DataFim)
        {
            LoteInternacao Lote = new LoteInternacao();

            string connectionString = "Provider=OraOLEDB.Oracle.1;Data Source=producao.world;User ID=GERADOR_DRG;Password=GERADOR_DRG";

            string queryInternacao = $@"SELECT SITUACAO ,
                                            CDTIPOINTERNACAO ,
                                            NUMEROOPERADORA ,
                                            NUMEROREGISTRO ,
                                            NUMEROATENDIMENTO ,
                                            NUMEROAUTORIZACAO ,
                                            DATAINTERNACAO ,
                                            DATAALTA ,
                                            CDMOTALT ,
                                            CODIGOCIDPRINCIPAL ,
                                            DATAAUTORIZACAO ,
                                            INTERNADOOUTRASVEZES ,
                                            HOSPITALINTERNACAOANTERIOR ,
                                            REINTERNACAO ,
                                            RECAIDA ,
                                            ACAO FROM dbamv.VIEW_GERADOR_DRG_INTERNACAO
                                            where DATABUSCA BETWEEN To_Date('{DataInicio.ToString("dd/MM/yyyy")}', 'dd/mm/yyyy') AND To_Date('{DataFim.ToString("dd/MM/yyyy")}', 'dd/mm/yyyy')
                                            ";

            string queryBenificiario = @"SELECT CODIGO ,
                                        NOME ,
                                        DATANASCIMENTO ,
                                        SEXO ,
                                        NOMEMAE ,
                                        CPF ,
                                        ENDERECO ,
                                        RECEMNASCIDO ,
                                        PARTICULAR  FROM DBAMV.VIEW_GERADOR_DRG_BENEFICIARIO
                                        where CD_ATENDIMENTO = ?";

            string queryOperadora = @"SELECT CODIGO ,
                                        NOME ,
                                        SIGLA ,
                                        PLANO ,
                                        NUMEROCARTEIRA ,
                                        DATAVALIDADE,
                                        TIPO FROM dbamv.VIEW_GERADOR_DRG_OPERADORA
                                        where CD_ATENDIMENTO = ?";

            string queryMedicos = @"
                    SELECT CODIGO,
                    NOME ,
                    DDD ,
                    TELEFONE ,
                    EMAIL ,
                    UF ,
                    CRM ,
                    ESPECIALIDADE ,
                    MEDICORESPONSAVEL ,
                    TIPOATUACAO  FROM DBAMV.VIEW_GERADOR_DRG_MEDICOS
                    where CD_ATENDIMENTO = ?";

            string queryProcedimentos = @"
                SELECT CODIGOPROCEDIMENTO ,
                DATAAUTORIZACAO ,
                DATAEXECUCAO ,
                DATAEXECUCAOFINAL
                from  dbamv.VIEW_GERADOR_DRG_PROCEDIMENTOS
                where CD_ATENDIMENTO = ?";

            string queryCidSecundario = @"
               SELECT CODIGOCIDSECUNDARIO
                from DBAMV.VIEW_GERADOR_DRG_CIDSECUNDARIO
                where CD_ATENDIMENTO = ?";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var command = new OleDbCommand();

                #region Internaçao
                command = new OleDbCommand(queryInternacao, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        Lote.Internacoes.Add(new LoteInternacao.Internacao
                        {
                            Situacao = reader["Situacao"].ToString()
                            ,
                            CdTipoInternacao = reader["CdTipoInternacao"].ToString()
                            ,
                            NumeroOperadora = reader["NumeroOperadora"].ToString()
                            ,
                            NumeroRegistro = reader["NumeroRegistro"].ToString()
                            ,
                            NumeroAtendimento = reader["NumeroAtendimento"].ToString()
                            ,
                            NumeroAutorizacao = reader["NumeroAutorizacao"].ToString()
                            ,
                            DataInternacao = reader["DataInternacao"].ToString()
                            ,
                            DataAlta = reader["DataAlta"].ToString()
                            ,
                            CdMotAlt = reader["CdMotAlt"].ToString()
                            ,
                            DataAutorizacao = reader["DataAutorizacao"].ToString()
                            ,
                            CodigoCidPrincipal = reader["CodigoCidPrincipal"].ToString()
                            ,
                            InternadoOutrasVezes = reader["InternadoOutrasVezes"].ToString()
                            ,
                            HospitalInternacaoAnterior = reader["HospitalInternacaoAnterior"].ToString()
                            ,
                            Reinternacao = reader["Reinternacao"].ToString()
                            ,
                            Recaida = reader["Recaida"].ToString()
                            ,
                            Acao = reader["Acao"].ToString()
                        });
                    }
                } 
                #endregion

                foreach (var atendimento in Lote.Internacoes)
                {
                    #region Benificiario
                    command = new OleDbCommand(queryBenificiario, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                       {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),
                       });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.Beneficiario = new LoteInternacao.Beneficiario
                            {
                                Codigo = reader["Codigo"].ToString()
                                ,
                                Nome = reader["Nome"].ToString()
                                ,
                                DataNascimento = reader["dataNascimento"].ToString()
                                ,
                                Sexo = reader["sexo"].ToString()
                                ,
                                NomeMae = reader["NomeMae"].ToString()
                                ,
                                Cpf = reader["cpf"].ToString()
                                ,
                                Endereco = reader["Endereco"].ToString()
                                ,
                                RecemNascido = reader["recemNascido"].ToString()
                                ,
                                Particular = reader["Particular"].ToString()
                            };
                        }
                    } 
                    #endregion

                    #region Operadora
                    command = new OleDbCommand(queryOperadora, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                        {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.Operadora = new LoteInternacao.Operadora
                            {
                                Codigo = reader["Codigo"].ToString()
                                ,
                                Nome = reader["Nome"].ToString()
                                ,
                                Sigla = reader["sigla"].ToString()
                                ,
                                Plano = reader["plano"].ToString()
                                ,
                                NumeroCarteira = reader["NumeroCarteira"].ToString()
                                ,
                                DataValidade = reader["DataValidade"].ToString()
                                ,
                                Tipo = reader["Tipo"].ToString()
                            };
                        }
                    }
                    #endregion

                    #region Medicos
                    command = new OleDbCommand(queryMedicos, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                        {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.Medicos.Add(new LoteInternacao.Medico
                            {
                                Codigo = reader["Codigo"].ToString()
                                ,
                                Nome = reader["Nome"].ToString()
                                ,
                                Ddd = reader["ddd"].ToString()
                                ,
                                Telefone = reader["telefone"].ToString()
                                ,
                                Email = reader["email"].ToString()
                                ,
                                Uf = reader["Uf"].ToString()
                                ,
                                Crm = reader["Crm"].ToString()
                                ,
                                Especialidade = reader["Especialidade"].ToString()
                                ,
                                MedicoResponsavel = reader["medicoResponsavel"].ToString()
                                ,
                                TipoAtuacao = reader["tipoAtuacao"].ToString()
                            });
                        }
                    }
                    #endregion

                    #region Procedimentos
                    command = new OleDbCommand(queryProcedimentos, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                        {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.Procedimentos.Add(new LoteInternacao.Procedimento
                            {
                                CodigoProcedimento = reader["CodigoProcedimento"].ToString()
                                ,
                                DataAutorizacao = reader["dataAutorizacao"].ToString()
                                ,
                                DataExecucao = reader["DataExecucao"].ToString()
                                ,
                                DataExecucaoFinal = reader["dataExecucaoFinal"].ToString()
                            });
                        }
                    }
                    #endregion

                    #region CidSecundario
                    command = new OleDbCommand(queryCidSecundario, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                        {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),
                        });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.CidSecundarios.Add(new LoteInternacao.CidSecundario
                            {
                                CodigoCidSecundario = reader["CodigoCidSecundario"].ToString()
                            });
                        }
                    } 
                    #endregion

                }

                connection.Close();
            }

            return Lote;
        }
    }
}