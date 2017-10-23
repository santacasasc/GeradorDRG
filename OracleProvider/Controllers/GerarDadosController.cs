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
    public class GerarDadosController : ApiController
    {
        // GET: GerarDados

        public LoteInternacao Get([FromUri] DateTime DataInicio, [FromUri] DateTime DataFim)
        {
            LoteInternacao Lote = new LoteInternacao();

            string connectionString = "Provider=OraOLEDB.Oracle.1;Data Source=producao.world;User ID=GERADOR_DRG;Password=GERADOR_DRG";

            string queryInternacao = $@"SELECT SITUACAO ,
                                            CARATERINTERNACAO ,
                                            NUMEROOPERADORA ,
                                            NUMEROREGISTRO ,
                                            NUMEROATENDIMENTO ,
                                            NUMEROAUTORIZACAO ,
                                            DATAINTERNACAO ,
                                            DATAALTA ,
                                            CONDICAOALTA ,
                                            CODIGOCIDPRINCIPAL ,
                                            DATAAUTORIZACAO ,
                                            INTERNADOOUTRASVEZES ,
                                            HOSPITALINTERNACAOANTERIOR ,
                                            ULTIMAINTERNACAO30DIAS ,
                                            INTERNACAORECAIDA ,
                                            ACAO  FROM dbamv.VIEW_GERADOR_DRG_INTERNACAO
                                            where DATAALTA BETWEEN To_Date('{DataInicio.ToString("dd/MM/yyyy")}', 'dd/mm/yyyy') AND To_Date('{DataFim.ToString("dd/MM/yyyy")}', 'dd/mm/yyyy')
                                            ";

            string queryBenificiario = @"SELECT CODIGOBENEFICIARIO ,
                                        NOMEPACIENTE ,
                                        DATANASCIMENTO ,
                                        SEXO ,
                                        NOMEMAE ,
                                        CPF ,
                                        ENDERECO ,
                                        RECEMNASCIDO ,
                                        PARTICULAR  FROM DBAMV.VIEW_GERADOR_DRG_BENEFICIARIO
                                        where CD_ATENDIMENTO = ?";

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var command = new OleDbCommand(queryInternacao, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        Lote.Internacoes.Add(new LoteInternacao.Internacao { Situacao = reader["Situacao"].ToString()
                            , CaraterInternacao = reader["CaraterInternacao"].ToString()
                            , NumeroAtendimento = reader["NumeroAtendimento"].ToString()
                        });
                    }
                }

                foreach (var atendimento in Lote.Internacoes)
                {
                    command = new OleDbCommand(queryBenificiario, connection);

                    command.Parameters.AddRange(new OleDbParameter[]
                       {
                           new OleDbParameter("@CD_ATENDIMENTO", atendimento.NumeroAtendimento),

                       });

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            atendimento.Beneficiarios = new LoteInternacao.Internacao.Beneficiario
                            {
                                Codigo =reader["CODIGOBENEFICIARIO"].ToString(),
                                Nome = reader["NOMEPACIENTE"].ToString()
                            };
                        }
                    }
                }

                connection.Close();
            }

            return Lote;
        }
    }
}