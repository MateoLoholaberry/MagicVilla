using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class actualizandoCampoEnNumVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaEdicion",
                table: "NumeroVillas",
                newName: "FechaActualizacion");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 20, 21, 19, 6, 818, DateTimeKind.Local).AddTicks(1660), new DateTime(2024, 4, 20, 21, 19, 6, 818, DateTimeKind.Local).AddTicks(1652) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 20, 21, 19, 6, 818, DateTimeKind.Local).AddTicks(1663), new DateTime(2024, 4, 20, 21, 19, 6, 818, DateTimeKind.Local).AddTicks(1662) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaActualizacion",
                table: "NumeroVillas",
                newName: "FechaEdicion");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 20, 20, 30, 18, 19, DateTimeKind.Local).AddTicks(8091), new DateTime(2024, 4, 20, 20, 30, 18, 19, DateTimeKind.Local).AddTicks(8083) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 20, 20, 30, 18, 19, DateTimeKind.Local).AddTicks(8094), new DateTime(2024, 4, 20, 20, 30, 18, 19, DateTimeKind.Local).AddTicks(8093) });
        }
    }
}
