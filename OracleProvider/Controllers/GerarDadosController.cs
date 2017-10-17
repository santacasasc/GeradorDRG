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

        public LoteInternacao Get(DateTime dataIncial,DateTime dataFinal)
        {
            LoteInternacao Lote = new LoteInternacao();

            string connectionString = "";

            string queryInternacao = $@"select
                                      CASE
                                        WHEN a.dt_alta IS NULL THEN '1'
                                        ELSE '3'
                                      END AS Situacao,

                                     CASE
                                        WHEN b.TP_INTERNACAO IN('1','7','8') THEN '1'
                                        WHEN b.TP_INTERNACAO IN('2','4') THEN '2'
                                      END AS CaraterInternacao,

                                      CASE
                                        WHEN a.cd_convenio=01 THEN '1'
                                        WHEN a.cd_convenio=40 THEN ''
                                        ELSE c.nr_registro_operadora_ans
                                      END AS NumeroOperadora,

                                      a.cd_paciente NumeroRegistro,
                                      a.cd_atendimento NumeroAtendimento,
                                      '' NumeroAutorizacao,
                                      a.dt_atendimento DataInternacao,
                                      a.hr_alta DataAlta,

                                      CASE
                                        WHEN a.cd_mot_alt IN(53)          THEN 'A'
                                        WHEN a.cd_mot_alt IN(10)          THEN 'I'
                                        WHEN a.cd_mot_alt IN(54)          THEN 'D'
                                        WHEN a.cd_mot_alt IN(4)           THEN 'P'
                                        WHEN a.cd_mot_alt IN(51,52,12,23) THEN 'O'
                                        WHEN a.cd_mot_alt IN(6)           THEN 'E'
                                        ELSE 'X'
                                      END AS CondicaoAlta,

                                      a.cd_cid AS CodigoCidPrincipal,
                                      To_Char(a.dt_atendimento, 'yyyy-MM-dd') || 'T' || To_Char(a.hr_atendimento, 'hh24:mi:ss') DataAutorizacao,

                                      CASE
                                        WHEN (SELECT
                                                Coalesce(Count(*),0) AS Qtde
                                              FROM
                                                atendime ate
                                              WHERE
                                                ate.cd_paciente=a.cd_paciente
                                                AND ate.dt_alta < a.dt_alta
                                                AND ate.tp_atendimento='I') > 0 THEN 'N'
                                        ELSE null
                                      END AS InternadoOutrasVezes,

                                      CASE
                                        WHEN (SELECT
                                                Coalesce(Count(*),0) AS Qtde
                                              FROM
                                                atendime ate
                                              WHERE
                                                ate.cd_paciente=a.cd_paciente
                                                AND ate.dt_alta < a.dt_alta
                                                AND ate.tp_atendimento='I') > 0 THEN 'N'
                                        ELSE null
                                      END AS HospitalInternacaoAnterior,

                                      CASE
                                        WHEN (a.dt_alta - (SELECT
                                                             ult.dt_alta
                                                           FROM
                                                             ATENDIME ult
                                                           WHERE
                                                             ult.cd_paciente=a.cd_paciente
                                                             AND ult.dt_alta < a.dt_alta
                                                             AND ROWNUM=1)) > 30 THEN 'S'
                                        ELSE 'N'
                                      END AS UltimaInternacao30Dias,

                                      CASE
                                        WHEN a.cd_cid = (SELECT
                                                           ult.cd_cid
                                                         FROM
                                                           atendime ult
                                                         WHERE
                                                           ult.cd_paciente=a.cd_paciente
                                                           AND ult.dt_alta < a.dt_alta
                                                           AND ROWNUM=1) THEN 'S'
                                        ELSE 'N'
                                      END AS InternacaoRecaida,
                                      'INCLUIR' AS Acao
                                    FROM
                                      atendime             a
                                      JOIN tipo_internacao b ON( b.cd_tipo_internacao=a.cd_tipo_internacao)
                                      JOIN convenio        c ON( c.cd_convenio=a.cd_convenio)
                                      JOIN paciente        e ON( e.cd_paciente=a.cd_paciente)
                                    WHERE
                                      trunc(a.dt_alta) BETWEEN To_Date('{dataIncial.ToString("dd/mm/yyyy")}, 'dd/mm/yyyy') AND To_Date('{dataFinal.ToString("dd/mm/yyyy")}', 'dd/mm/yyyy')
                                      AND a.tp_atendimento='I'";

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

                }

                connection.Close();
            }

            return Lote;
        }
    }
}