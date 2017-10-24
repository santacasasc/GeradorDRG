CREATE USER gerador_drg IDENTIFIED BY "GERADOR_DRG"
    DEFAULT TABLESPACE "MV2000_D"
    TEMPORARY TABLESPACE "TEMP";

GRANT "CONNECT" TO "GERADOR_DRG";


  CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" ("SITUACAO", "CARATERINTERNACAO", "NUMEROOPERADORA", "NUMEROREGISTRO", "NUMEROATENDIMENTO", "NUMEROAUTORIZACAO", "DATAINTERNACAO", "DATAALTA", "CONDICAOALTA", "CODIGOCIDPRINCIPAL", "DATAAUTORIZACAO", "INTERNADOOUTRASVEZES", "HOSPITALINTERNACAOANTERIOR", "REINTERNACAO", "RECAIDA", "ACAO") AS 
  select
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
  END AS reinternacao,

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
  END AS recaida,
  'INCLUIR' AS Acao
FROM
  atendime             a
  JOIN tipo_internacao b ON( b.cd_tipo_internacao=a.cd_tipo_internacao)
  JOIN convenio        c ON( c.cd_convenio=a.cd_convenio)
  JOIN paciente        e ON( e.cd_paciente=a.cd_paciente)
WHERE
  a.tp_atendimento='I';



  CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" ("CODIGO", "NOME", "DATANASCIMENTO", "SEXO", "NOMEMAE", "CPF", "ENDERECO", "RECEMNASCIDO", "PARTICULAR", "CD_ATENDIMENTO") AS 
  SELECT
  CASE
    WHEN a.cd_convenio IN( 1) THEN b.nr_cns
    WHEN a.cd_convenio IN(40) THEN ''
    ELSE c.nr_carteira
  END AS codigo,
  b.nm_paciente AS nome,
  To_Char(b.dt_nascimento, 'yyyy-MM-dd') || 'T' || '00:00:00' AS dataNascimento,
  b.tp_sexo AS sexo,
  b.nm_mae AS nomeMae,
  b.nr_cpf AS cpf,
  b.ds_endereco || ' ' || nr_endereco || ', ' || b.nm_bairro || ' - ' || d.nm_cidade || ' - ' || b.nr_cep AS endereco,
  'N' AS recemNascido,
  CASE
    WHEN a.cd_convenio IN(40) THEN 'S'
    ELSE 'N'
  END AS particular,
  a.CD_ATENDIMENTO CD_ATENDIMENTO
FROM
  atendime      a
  JOIN paciente b ON( b.cd_paciente=a.cd_paciente)
  LEFT
  JOIN carteira c ON( c.cd_paciente=a.cd_paciente
                  AND c.cd_convenio=a.cd_convenio)
  JOIN cidade   d ON( d.cd_cidade=b.cd_cidade);

  
CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_OPERADORA" ("CODIGO", "NOME", "SIGLA", "PLANO", "NUMEROCARTEIRA", "DATAVALIDADE", "TIPO", "CD_ATENDIMENTO") AS 
  select
  CASE
    WHEN a.cd_convenio=01 THEN '1'
    WHEN a.cd_convenio=40 THEN ''
    ELSE b.nr_registro_operadora_ans
  END AS codigo,

  b.nm_convenio AS nome,
  SubStr(b.nm_convenio, 1, 9) AS sigla,
  'UNICO' AS plano,

  CASE
    WHEN a.cd_convenio IN( 1) THEN d.nr_cns
    WHEN a.cd_convenio IN(40) THEN ''
    ELSE c.nr_carteira
  END AS numeroCarteira,

  CASE
    WHEN a.cd_convenio IN(1,40) THEN ''
    ELSE To_Char(c.dt_validade, 'yyyy-MM-dd') || 'T' || To_Char(c.dt_validade, 'hh24:mi:ss') 
  END AS dataValidade,
  CASE
    WHEN a.cd_convenio=01 THEN 'SUS'
    WHEN a.cd_convenio=40 THEN 'CMS'
    ELSE 'TUSS'
  END AS Tipo,
  a.CD_ATENDIMENTO CD_ATENDIMENTO
FROM
  atendime      a
  JOIN convenio b ON(b.cd_convenio=a.cd_convenio)
  LEFT
  JOIN carteira c ON( c.cd_paciente=a.cd_paciente
                  AND c.cd_convenio=a.cd_convenio)
  JOIN paciente d ON( d.cd_paciente=a.cd_paciente);



  
  CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_MEDICOS" ("NOME", "DDD", "TELEFONE", "EMAIL", "UF", "CRM", "ESPECIALIDADE", "MEDICORESPONSAVEL", "TIPOATUACAO", "CD_ATENDIMENTO") AS 
  SELECT
  b.nm_prestador AS nome,
  CASE
    WHEN c.nr_ddi_celular is not null THEN c.nr_ddi_celular
    ELSE '16'
  END AS DDD,
  c.ds_tip_comun_prest AS telefone,
  b.ds_email AS email,
  b.cd_uf_orgao_emissor AS uf,
  b.ds_codigo_conselho AS crm,
  d.ds_especialid AS especialidade,
  'S' AS MedicoResponsavel,
  'C' AS TipoAtuacao,
  a.CD_ATENDIMENTO
FROM
  atendime                 a
  JOIN prestador           b ON( b.cd_prestador=a.cd_prestador)
  LEFT  
  JOIN prestador_tip_comun c ON( c.cd_prestador=a.cd_prestador
                             AND c.cd_tip_comun=3)
  JOIN especialid          d ON( d.cd_especialid=a.cd_especialid);

  CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_PROCEDIMENTOS" ("CODIGOPROCEDIMENTO", "DATAAUTORIZACAO", "DATAEXECUCAO", "DATAEXECUCAOFINAL", "CD_ATENDIMENTO") AS 
  select
  CASE
    WHEN a.cd_convenio IN (1) THEN a.cd_procedimento
    ELSE a.cd_pro_int
  END AS CodigoProcedimento,

  To_Char(a.dt_atendimento, 'yyyy-MM-dd') || 'T' || To_Char(a.hr_atendimento, 'hh24:mi:ss') DataAutorizacao,
  To_Char(a.dt_atendimento, 'yyyy-MM-dd') || 'T' || To_Char(a.hr_atendimento, 'hh24:mi:ss') DataExecucao,
  To_Char(a.dt_atendimento, 'yyyy-MM-dd') || 'T' || To_Char(a.hr_atendimento, 'hh24:mi:ss') dataExecucaoFinal,
  a.cd_atendimento
FROM
  atendime a;

    CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_CIDSECUNDARIO" ("CODIGOCIDSECUNDARIO", "CD_ATENDIMENTO") AS 
  select "CODIGOCIDSECUNDARIO","CD_ATENDIMENTO" from (SELECT '' codigoCidSecundario,'' Cd_atendimento
    
FROM sys.dual)
where codigoCidSecundario != '';


  CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_CTI" ("DATAINICIAL", "DATAFINAL", "CODIGOCIDPRINCIPAL", "CONDICAOALTA", "UF", "CRM", "CODIGOHOSPITAL", "NOMEHOSPITAL", "TIPO", "CD_ATENDIMENTO") AS 
  select "DATAINICIAL","DATAFINAL","CODIGOCIDPRINCIPAL","CONDICAOALTA","UF","CRM","CODIGOHOSPITAL","NOMEHOSPITAL","TIPO","CD_ATENDIMENTO" from 
(SELECT '' dataInicial,'' dataFinal, '' codigoCidPrincipal, '' condicaoAlta
, '' uf, '' crm, '' codigoHospital, '' nomeHospital, '' tipo, '' cd_atendimento
    
FROM sys.dual)
where dataInicial != '';


GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_OPERADORA" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_MEDICOS" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_PROCEDIMENTOS" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_CIDSECUNDARIO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_CTI" TO gerador_drg;