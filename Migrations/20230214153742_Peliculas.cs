using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Peliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Generos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Actores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "foto",
                table: "Actores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "biografia",
                table: "Actores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    resumen = table.Column<string>(type: "longtext", nullable: true),
                    trailer = table.Column<string>(type: "longtext", nullable: true),
                    enCines = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fechaLanzamiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    poster = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PeliculasActores",
                columns: table => new
                {
                    peliculaId = table.Column<int>(type: "int", nullable: false),
                    actorId = table.Column<int>(type: "int", nullable: false),
                    personaje = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasActores", x => new { x.actorId, x.peliculaId });
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Actores_actorId",
                        column: x => x.actorId,
                        principalTable: "Actores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Peliculas_peliculaId",
                        column: x => x.peliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PeliculasGeneros",
                columns: table => new
                {
                    peliculaId = table.Column<int>(type: "int", nullable: false),
                    generoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasGeneros", x => new { x.generoId, x.peliculaId });
                    table.ForeignKey(
                        name: "FK_PeliculasGeneros_Generos_generoId",
                        column: x => x.generoId,
                        principalTable: "Generos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasGeneros_Peliculas_peliculaId",
                        column: x => x.peliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_peliculaId",
                table: "PeliculasActores",
                column: "peliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasGeneros_peliculaId",
                table: "PeliculasGeneros",
                column: "peliculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculasActores");

            migrationBuilder.DropTable(
                name: "PeliculasGeneros");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Generos",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Actores",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "foto",
                table: "Actores",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "biografia",
                table: "Actores",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
