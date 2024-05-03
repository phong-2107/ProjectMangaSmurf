using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMangaSmurf.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "gia_thanh",
                table: "Premium",
                type: "decimal(6,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "Premium",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "giayphep",
                table: "Footer",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Premium",
                table: "Premium",
                column: "gia_thanh");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Footer",
                table: "Footer",
                column: "giayphep");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Premium",
                table: "Premium");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Footer",
                table: "Footer");

            migrationBuilder.AlterColumn<bool>(
                name: "active",
                table: "Premium",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "gia_thanh",
                table: "Premium",
                type: "decimal(6,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,0)");

            migrationBuilder.AlterColumn<string>(
                name: "giayphep",
                table: "Footer",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);
        }
    }
}
