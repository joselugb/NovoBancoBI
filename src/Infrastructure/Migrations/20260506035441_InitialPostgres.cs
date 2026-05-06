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
                    id_cliente = table.Column<Guid>(type: "uuid", nullable: false),
                    numero_cuenta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    balance = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    estado_cuenta = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    moneda = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuenta", x => x.Id);
                    table.CheckConstraint("CK_Cuentas_Saldo_NoNegativo", "balance >= 0");
                    table.ForeignKey(
                        name: "FK_Cuentas_Clientes",
                        column: x => x.id_cliente,
                        principalTable: "clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transacciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    id_cuenta_origen = table.Column<Guid>(type: "uuid", nullable: false),
                    id_cuenta_destino = table.Column<Guid>(type: "uuid", nullable: false),
                    monto = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_transaccion = table.Column<int>(type: "integer", nullable: false),
                    estado_transaccion = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Cuentas",
                        column: x => x.id_cuenta_origen,
                        principalTable: "cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_DocumentoIdentidad",
                table: "clientes",
                column: "documento_identidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdCliente",
                table: "cuenta",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_NumeroCuenta",
                table: "cuenta",
                column: "numero_cuenta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdCuentaOrigen_Fecha",
                table: "transacciones",
                columns: new[] { "id_cuenta_origen", "fecha" });

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
                name: "transacciones");

            migrationBuilder.DropTable(
                name: "cuenta");

            migrationBuilder.DropTable(
                name: "clientes");
        }
    }
}
