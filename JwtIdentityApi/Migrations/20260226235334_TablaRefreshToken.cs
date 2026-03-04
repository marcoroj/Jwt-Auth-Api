using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtIdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class TablaRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokensHistoriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechCreacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, computedColumnSql: "IIF(FechaExpiracion < GETDATE(), CONVERT(bit,0), CONVERT(bit,1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokensHistoriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokensHistoriales_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokensHistoriales_UsuarioId",
                table: "RefreshTokensHistoriales",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokensHistoriales");
        }
    }
}
