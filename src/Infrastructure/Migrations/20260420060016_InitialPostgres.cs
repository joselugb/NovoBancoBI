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
                name: "clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    documento_identidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cuenta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    numero_cuenta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    balance = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    estado_cuenta = table.Column<int>(type: "integer", nullable: false)
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
                    id_cuenta_origen = table.Column<int>(type: "integer", nullable: false),
                    id_cuenta_destino = table.Column<int>(type: "integer", nullable: false),
                    monto = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_transaccion = table.Column<int>(type: "integer", nullable: false),
                    estado_transaccion = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "IX_Transacciones_Referencia",
                table: "transacciones",
                column: "referencia",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "cuenta");

            migrationBuilder.DropTable(
                name: "transacciones");
        }
    }
}
