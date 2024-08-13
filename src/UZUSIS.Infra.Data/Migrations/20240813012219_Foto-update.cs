using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UZUSIS.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fotoupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotos_Produto_ProdutoId",
                table: "Fotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotos",
                table: "Fotos");

            migrationBuilder.DropColumn(
                name: "FotoBytes",
                table: "Fotos");

            migrationBuilder.RenameTable(
                name: "Fotos",
                newName: "Foto");

            migrationBuilder.RenameIndex(
                name: "IX_Fotos_ProdutoId",
                table: "Foto",
                newName: "IX_Foto_ProdutoId");

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Foto",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Foto",
                table: "Foto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foto_Produto_ProdutoId",
                table: "Foto",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foto_Produto_ProdutoId",
                table: "Foto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Foto",
                table: "Foto");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Foto");

            migrationBuilder.RenameTable(
                name: "Foto",
                newName: "Fotos");

            migrationBuilder.RenameIndex(
                name: "IX_Foto_ProdutoId",
                table: "Fotos",
                newName: "IX_Fotos_ProdutoId");

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoBytes",
                table: "Fotos",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotos",
                table: "Fotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotos_Produto_ProdutoId",
                table: "Fotos",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
