using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2024, 4, 19, 9, 20, 13, 935, DateTimeKind.Local).AddTicks(7362), new DateTime(2024, 4, 19, 9, 20, 13, 935, DateTimeKind.Local).AddTicks(7351), "", 50, "Villa Real", 5, 250.0 },
                    { 2, "", "Detalle de la villa...", new DateTime(2024, 4, 19, 9, 20, 13, 935, DateTimeKind.Local).AddTicks(7364), new DateTime(2024, 4, 19, 9, 20, 13, 935, DateTimeKind.Local).AddTicks(7364), "", 40, "Premiun vista a la piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
