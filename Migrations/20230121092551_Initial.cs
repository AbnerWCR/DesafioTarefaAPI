using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarefaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TAREFAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TITULO = table.Column<string>(type: "varchar(50)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "varchar(100)", nullable: false),
                    DATA = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAREFAS", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAREFAS");
        }
    }
}
