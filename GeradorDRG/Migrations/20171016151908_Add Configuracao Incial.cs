using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeradorDRG.Migrations
{
    public partial class AddConfiguracaoIncial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false),
                    Padrao = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false),
                    Padrao = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BancoId = table.Column<int>(type: "int", nullable: false),
                    BancoSenha = table.Column<string>(type: "longtext", nullable: true),
                    BancoUsuario = table.Column<string>(type: "longtext", nullable: true),
                    CodDRG = table.Column<string>(type: "longtext", nullable: true),
                    NomeDRG = table.Column<string>(type: "longtext", nullable: true),
                    SistemaId = table.Column<int>(type: "int", nullable: false),
                    UtilizaWebService = table.Column<bool>(type: "bit", nullable: false),
                    WebServiceSenha = table.Column<string>(type: "longtext", nullable: true),
                    WebServiceUsuario = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuracao_Banco_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuracao_Sistema_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SistemaBanco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BancoId = table.Column<int>(type: "int", nullable: false),
                    Padrao = table.Column<bool>(type: "bit", nullable: false),
                    SistemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SistemaBanco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SistemaBanco_Banco_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SistemaBanco_Sistema_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PacienteTeste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodPaciente = table.Column<string>(type: "longtext", nullable: true),
                    ConfiguracaoId = table.Column<int>(type: "int", nullable: false),
                    NomePaciente = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacienteTeste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PacienteTeste_Configuracao_ConfiguracaoId",
                        column: x => x.ConfiguracaoId,
                        principalTable: "Configuracao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrestadorTeste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodPrestador = table.Column<string>(type: "longtext", nullable: true),
                    ConfiguracaoId = table.Column<int>(type: "int", nullable: false),
                    NomePrestador = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestadorTeste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrestadorTeste_Configuracao_ConfiguracaoId",
                        column: x => x.ConfiguracaoId,
                        principalTable: "Configuracao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracao_BancoId",
                table: "Configuracao",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracao_SistemaId",
                table: "Configuracao",
                column: "SistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_PacienteTeste_ConfiguracaoId",
                table: "PacienteTeste",
                column: "ConfiguracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestadorTeste_ConfiguracaoId",
                table: "PrestadorTeste",
                column: "ConfiguracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SistemaBanco_BancoId",
                table: "SistemaBanco",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_SistemaBanco_SistemaId",
                table: "SistemaBanco",
                column: "SistemaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PacienteTeste");

            migrationBuilder.DropTable(
                name: "PrestadorTeste");

            migrationBuilder.DropTable(
                name: "SistemaBanco");

            migrationBuilder.DropTable(
                name: "Configuracao");

            migrationBuilder.DropTable(
                name: "Banco");

            migrationBuilder.DropTable(
                name: "Sistema");
        }
    }
}
