CREATE USER gerador_drg IDENTIFIED BY "GERADOR_DRG"
    DEFAULT TABLESPACE "MV2000_D"
    TEMPORARY TABLESPACE "TEMP";

GRANT "CONNECT" TO "GERADOR_DRG";

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" (
    "SITUACAO",
    "CARATERINTERNACAO",
    "NUMEROOPERADORA",
    "NUMEROREGISTRO",
    "NUMEROATENDIMENTO",
    "NUMEROAUTORIZACAO",
    "DATAINTERNACAO",
    "DATAALTA",
    "CONDICAOALTA",
    "CODIGOCIDPRINCIPAL",
    "DATAAUTORIZACAO",
    "INTERNADOOUTRASVEZES",
    "HOSPITALINTERNACAOANTERIOR",
    "ULTIMAINTERNACAO30DIAS",
    "INTERNACAORECAIDA",
    "ACAO"
) AS
    SELECT
            CASE
                WHEN a.dt_alta IS NULL THEN '1'
                ELSE '3'
            END
        AS situacao,
            CASE
                WHEN b.tp_internacao IN (
                    '1','7','8'
                ) THEN '1'
                WHEN b.tp_internacao IN (
                    '2','4'
                ) THEN '2'
            END
        AS caraterinternacao,
            CASE
                WHEN a.cd_convenio = 01   THEN '1'
                WHEN a.cd_convenio = 40   THEN ''
                ELSE c.nr_registro_operadora_ans
            END
        AS numerooperadora,
        a.cd_paciente numeroregistro,
        a.cd_atendimento numeroatendimento,
        '' numeroautorizacao,
        a.dt_atendimento datainternacao,
        a.hr_alta dataalta,
            CASE
                WHEN a.cd_mot_alt IN (
                    53
                ) THEN 'A'
                WHEN a.cd_mot_alt IN (
                    10
                ) THEN 'I'
                WHEN a.cd_mot_alt IN (
                    54
                ) THEN 'D'
                WHEN a.cd_mot_alt IN (
                    4
                ) THEN 'P'
                WHEN a.cd_mot_alt IN (
                    51,52,12,23
                ) THEN 'O'
                WHEN a.cd_mot_alt IN (
                    6
                ) THEN 'E'
                ELSE 'X'
            END
        AS condicaoalta,
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
                ) > 0 THEN 'N'
                ELSE NULL
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
        AS ultimainternacao30dias,
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
        AS internacaorecaida,
        'INCLUIR' AS acao
    FROM
        atendime a
        JOIN tipo_internacao b ON (
            b.cd_tipo_internacao = a.cd_tipo_internacao
        )
        JOIN convenio c ON (
            c.cd_convenio = a.cd_convenio
        )
        JOIN paciente e ON (
            e.cd_paciente = a.cd_paciente
        )
    WHERE
        a.tp_atendimento = 'I';

CREATE OR REPLACE FORCE VIEW "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" (
    "CODIGOBENEFICIARIO",
    "NOMEPACIENTE",
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
        AS codigobeneficiario,
        b.nm_paciente AS nomepaciente,
        TO_CHAR(b.dt_nascimento,'yyyy-MM-dd')
         || 'T'
         || '00:00:00' AS datanascimento,
        b.tp_sexo AS sexo,
        b.nm_mae AS nomemae,
        b.nr_cpf AS cpf,
        b.ds_endereco
         || ' '
         || nr_endereco
         || ','
         || b.nm_bairro
         || ' - '
         || d.nm_cidade
         || ' - '
         || b.nr_cep AS endereco,
        'N' AS recemnascido,
            CASE
                WHEN a.cd_convenio IN (
                    40
                ) THEN 'S'
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
        )
        JOIN cidade d ON (
            d.cd_cidade = b.cd_cidade
        );

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_INTERNACAO" TO gerador_drg;

GRANT SELECT ON "DBAMV"."VIEW_GERADOR_DRG_BENEFICIARIO" TO gerador_drg;