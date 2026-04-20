using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:estado_transaccion", "correcta,fallida,reversada")
                .Annotation("Npgsql:Enum:estados_cuenta", "activa,bloqueada,cerrada")
                .Annotation("Npgsql:Enum:tipo_cuenta", "ahorros,corriente")
                .Annotation("Npgsql:Enum:tipos_transacciones", "deposito,retiro,transferencia");

            migrationBuilder.CreateTable(
                name: "cuenta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    EstadosCuenta = table.Column<int>(type: "integer", nullable: false),
                    numero_cuenta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuenta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transacciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    Referencia = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacciones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cuenta_numero_cuenta",
                table: "cuenta",
                column: "numero_cuenta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transacciones_Referencia",
                table: "transacciones",
                column: "Referencia",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cuenta");

            migrationBuilder.DropTable(
                name: "transacciones");
        }
    }
}
