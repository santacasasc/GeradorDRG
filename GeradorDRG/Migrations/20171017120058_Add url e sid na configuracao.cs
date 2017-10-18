using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeradorDRG.Migrations
{
    public partial class Addurlesidnaconfiguracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BancoSID",
                table: "Configuracao",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BancoURL",
                table: "Configuracao",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BancoSID",
                table: "Configuracao");

            migrationBuilder.DropColumn(
                name: "BancoURL",
                table: "Configuracao");
        }
    }
}
