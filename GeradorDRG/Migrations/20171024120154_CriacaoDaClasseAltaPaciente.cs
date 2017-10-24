using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeradorDRG.Migrations
{
    public partial class CriacaoDaClasseAltaPaciente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoAlta",
                table: "Configuracao",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoAlta",
                table: "Configuracao",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoAlta",
                table: "Configuracao");

            migrationBuilder.DropColumn(
                name: "TipoAlta",
                table: "Configuracao");
        }
    }
}
