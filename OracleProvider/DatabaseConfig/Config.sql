CREATE USER gerador_drg IDENTIFIED BY "GERADOR_DRG"
    DEFAULT TABLESPACE "MV2000_D"
    TEMPORARY TABLESPACE "TEMP";

GRANT "CONNECT" TO "GERADOR_DRG";

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_TP_INTERNACAO" (
    "CODIGOTIPO",
    "TIPO"
) AS
    SELECT
        cd_tipo_internacao codigotipo,
        ds_tipo_internacao tipo
    FROM
        tipo_internacao;

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_MOTIVO_ALTA" (
    "CODIGOMOTIVO",
    "DSMOTIVOALTA"
) AS
    SELECT
        cd_mot_alt codigomotivo,
        ds_mot_alt dsmotivoalta
    FROM
        mot_alt;

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" (
    "SITUACAO",
    "CDTIPOINTERNACAO",
    "NUMEROOPERADORA",
    "NUMEROREGISTRO",
    "NUMEROATENDIMENTO",
    "NUMEROAUTORIZACAO",
    "DATAINTERNACAO",
    "DATAALTA",
    "CDMOTALT",
    "CODIGOCIDPRINCIPAL",
    "DATAAUTORIZACAO",
    "INTERNADOOUTRASVEZES",
    "HOSPITALINTERNACAOANTERIOR",
    "REINTERNACAO",
    "RECAIDA",
    "ACAO",
    "DATABUSCA"
) AS
    SELECT
            CASE
                WHEN a.dt_alta IS NULL THEN '1'
                ELSE '3'
            END
        AS situacao,
        a.cd_tipo_internacao AS cdtipointernacao,
            CASE
                WHEN a.cd_convenio = 01   THEN '1'
                WHEN a.cd_convenio = 40   THEN ''
                ELSE c.nr_registro_operadora_ans
            END
        AS numerooperadora,
        a.cd_paciente numeroregistro,
        a.cd_atendimento numeroatendimento,
        '' numeroautorizacao,
        TO_CHAR(a.dt_atendimento,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_atendimento,'hh24:mi:ss') datainternacao,
        TO_CHAR(a.dt_alta,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_alta,'hh24:mi:ss') dataalta,
        a.cd_mot_alt AS cdmotalt,
        a.cd_cid AS codigocidprincipal,
        TO_CHAR(a.dt_atendimento,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_atendimento,'hh24:mi:ss') dataautorizacao,
            CASE
                WHEN (
                    SELECT
                        coalesce(COUNT(*),0) AS qtde
                    FROM
                        atendime ate
                    WHERE
                            ate.cd_paciente = a.cd_paciente
                        AND
                            ate.dt_alta < a.dt_alta
                        AND
                            ate.tp_atendimento = 'I'
                ) > 0 THEN 'S'
                ELSE 'N'
            END
        AS internadooutrasvezes,
            CASE
                WHEN (
                    SELECT
                        coalesce(COUNT(*),0) AS qtde
                    FROM
                        atendime ate
                    WHERE
                            ate.cd_paciente = a.cd_paciente
                        AND
                            ate.dt_alta < a.dt_alta
                        AND
                            ate.tp_atendimento = 'I'
                ) > 0 THEN 'N'
                ELSE NULL
            END
        AS hospitalinternacaoanterior,
            CASE
                WHEN ( a.dt_alta - (
                    SELECT
                        ult.dt_alta
                    FROM
                        atendime ult
                    WHERE
                            ult.cd_paciente = a.cd_paciente
                        AND
                            ult.dt_alta < a.dt_alta
                        AND
                            ROWNUM = 1
                ) ) > 30 THEN 'S'
                ELSE 'N'
            END
        AS reinternacao,
            CASE
                WHEN a.cd_cid = (
                    SELECT
                        ult.cd_cid
                    FROM
                        atendime ult
                    WHERE
                            ult.cd_paciente = a.cd_paciente
                        AND
                            ult.dt_alta < a.dt_alta
                        AND
                            ROWNUM = 1
                ) THEN 'S'
                ELSE 'N'
            END
        AS recaida,
        'INCLUIR' AS acao,
        trunc(a.dt_alta) databusca
    FROM
        atendime a
        JOIN convenio c ON (
            c.cd_convenio = a.cd_convenio
        )
        JOIN paciente e ON (
            e.cd_paciente = a.cd_paciente
        )
    WHERE
        a.tp_atendimento = 'I';

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" (
    "CODIGO",
    "NOME",
    "DATANASCIMENTO",
    "SEXO",
    "NOMEMAE",
    "CPF",
    "ENDERECO",
    "RECEMNASCIDO",
    "PARTICULAR",
    "CD_ATENDIMENTO"
) AS
    SELECT
            CASE
                WHEN a.cd_convenio IN (
                    1
                ) THEN b.nr_cns
                WHEN a.cd_convenio IN (
                    40
                ) THEN ''
                ELSE c.nr_carteira
            END
        AS codigo,
        b.nm_paciente AS nome,
        TO_CHAR(b.dt_nascimento,'yyyy-MM-dd')
         || 'T'
         || '00:00:00' AS datanascimento,
        b.tp_sexo AS sexo,
        b.nm_mae AS nomemae,
        b.nr_cpf AS cpf,
        b.ds_endereco
         || ' '
         || b.nr_endereco
         || ','
         || b.nm_bairro
         || ' - '
         || d.nm_cidade
         || ' - '
         || b.nr_cep AS endereco,
            CASE
                WHEN a.cd_atendimento_pai IS NULL THEN 'N'
                ELSE 'S'
            END
        AS recemnascido,
            CASE
                WHEN e.tp_convenio = 'P' THEN 'S'
                ELSE 'N'
            END
        AS particular,
        a.cd_atendimento cd_atendimento
    FROM
        atendime a
        JOIN paciente b ON (
            b.cd_paciente = a.cd_paciente
        )
        LEFT JOIN carteira c ON (
                c.cd_paciente = a.cd_paciente
            AND
                c.cd_convenio = a.cd_convenio
            AND
                c.sn_carteira_ativo = 'S'
        )
        JOIN cidade d ON (
            d.cd_cidade = b.cd_cidade
        )
        INNER JOIN convenio e ON e.cd_convenio = a.cd_convenio;

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_OPERADORA" (
    "CODIGO",
    "NOME",
    "SIGLA",
    "PLANO",
    "NUMEROCARTEIRA",
    "DATAVALIDADE",
    "TIPO",
    "CD_ATENDIMENTO"
) AS
    SELECT
            CASE
                WHEN a.cd_convenio = 01   THEN '1'
                WHEN a.cd_convenio = 40   THEN ''
                ELSE b.nr_registro_operadora_ans
            END
        AS codigo,
        b.nm_convenio AS nome,
        substr(
            b.nm_convenio,
            1,
            9
        ) AS sigla,
        'UNICO' AS plano,
            CASE
                WHEN a.cd_convenio IN (
                    1
                ) THEN d.nr_cns
                WHEN a.cd_convenio IN (
                    40
                ) THEN ''
                ELSE c.nr_carteira
            END
        AS numerocarteira,
            CASE
                WHEN a.cd_convenio IN (
                    1,40
                ) THEN ''
                ELSE TO_CHAR(c.dt_validade,'yyyy-MM-dd')
                 || 'T'
                 || TO_CHAR(c.dt_validade,'hh24:mi:ss')
            END
        AS datavalidade,
            CASE
                WHEN a.cd_convenio = 01   THEN 'SUS'
                WHEN a.cd_convenio = 40   THEN 'CMS'
                ELSE 'TUSS'
            END
        AS tipo,
        a.cd_atendimento cd_atendimento
    FROM
        atendime a
        JOIN convenio b ON (
            b.cd_convenio = a.cd_convenio
        )
        LEFT JOIN carteira c ON (
                c.cd_paciente = a.cd_paciente
            AND
                c.cd_convenio = a.cd_convenio
        )
        JOIN paciente d ON (
            d.cd_paciente = a.cd_paciente
        );

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_MEDICOS" (
    "CODIGO",
    "NOME",
    "DDD",
    "TELEFONE",
    "EMAIL",
    "UF",
    "CRM",
    "ESPECIALIDADE",
    "MEDICORESPONSAVEL",
    "TIPOATUACAO",
    "CD_ATENDIMENTO"
) AS
    SELECT
        b.cd_prestador AS codigo,
        b.nm_prestador AS nome,
            CASE
                WHEN c.nr_ddi_celular IS NOT NULL THEN c.nr_ddi_celular
                ELSE '16'
            END
        AS ddd,
        c.ds_tip_comun_prest AS telefone,
        b.ds_email AS email,
        b.cd_uf_orgao_emissor AS uf,
        b.ds_codigo_conselho AS crm,
        d.ds_especialid AS especialidade,
        'S' AS medicoresponsavel,
        'C' AS tipoatuacao,
        a.cd_atendimento
    FROM
        atendime a
        JOIN prestador b ON (
            b.cd_prestador = a.cd_prestador
        )
        LEFT JOIN prestador_tip_comun c ON (
                c.cd_prestador = a.cd_prestador
            AND
                c.cd_tip_comun = 3
        )
        JOIN especialid d ON (
            d.cd_especialid = a.cd_especialid
        );

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_PROCEDIMENTOS" (
    "CODIGOPROCEDIMENTO",
    "DATAAUTORIZACAO",
    "DATAEXECUCAO",
    "DATAEXECUCAOFINAL",
    "CD_ATENDIMENTO"
) AS
    SELECT
            CASE
                WHEN a.cd_convenio IN (
                    1
                ) THEN a.cd_procedimento
                ELSE a.cd_pro_int
            END
        AS codigoprocedimento,
        TO_CHAR(a.dt_atendimento,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_atendimento,'hh24:mi:ss') dataautorizacao,
        TO_CHAR(a.dt_atendimento,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_atendimento,'hh24:mi:ss') dataexecucao,
        TO_CHAR(a.dt_atendimento,'yyyy-MM-dd')
         || 'T'
         || TO_CHAR(a.hr_atendimento,'hh24:mi:ss') dataexecucaofinal,
        a.cd_atendimento
    FROM
        atendime a;

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_CIDSECUNDARIO" (
    "CODIGOCIDSECUNDARIO",
    "CD_ATENDIMENTO"
) AS
    SELECT
        "CODIGOCIDSECUNDARIO",
        "CD_ATENDIMENTO"
    FROM
        (
            SELECT
                '' codigocidsecundario,
                '' cd_atendimento
            FROM
                sys.dual
        )
    WHERE
        codigocidsecundario != '';

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_CTI" (
    "DATAINICIAL",
    "DATAFINAL",
    "CODIGOCIDPRINCIPAL",
    "CONDICAOALTA",
    "UF",
    "CRM",
    "CODIGOHOSPITAL",
    "NOMEHOSPITAL",
    "TIPO",
    "CD_ATENDIMENTO"
) AS
    SELECT
        "DATAINICIAL",
        "DATAFINAL",
        "CODIGOCIDPRINCIPAL",
        "CONDICAOALTA",
        "UF",
        "CRM",
        "CODIGOHOSPITAL",
        "NOMEHOSPITAL",
        "TIPO",
        "CD_ATENDIMENTO"
    FROM
        (
            SELECT
                '' datainicial,
                '' datafinal,
                '' codigocidprincipal,
                '' condicaoalta,
                '' uf,
                '' crm,
                '' codigohospital,
                '' nomehospital,
                '' tipo,
                '' cd_atendimento
            FROM
                sys.dual
        )
    WHERE
        datainicial != '';

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_TP_INTERNACAO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_MOTIVO_ALTA" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_OPERADORA" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_MEDICOS" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_PROCEDIMENTOS" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_CIDSECUNDARIO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_CTI" TO gerador_drg;