using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeradorDRG.Migrations
{
    public partial class AdiçãodatabeladeMotivosdeAlta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoAlta",
                table: "Configuracao");

            migrationBuilder.DropColumn(
                name: "TipoAlta",
                table: "Configuracao");

            migrationBuilder.CreateTable(
                name: "AltaPaciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoMotivo = table.Column<string>(type: "longtext", nullable: true),
                    ConfiguracaoId = table.Column<int>(type: "int", nullable: false),
                    MotivoAlta = table.Column<string>(type: "longtext", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltaPaciente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AltaPaciente_Configuracao_ConfiguracaoId",
                        column: x => x.ConfiguracaoId,
                        principalTable: "Configuracao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AltaPaciente_ConfiguracaoId",
                table: "AltaPaciente",
                column: "ConfiguracaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltaPaciente");

            migrationBuilder.AddColumn<string>(
                name: "MotivoAlta",
                table: "Configuracao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoAlta",
                table: "Configuracao",
                nullable: true);
        }
    }
}
