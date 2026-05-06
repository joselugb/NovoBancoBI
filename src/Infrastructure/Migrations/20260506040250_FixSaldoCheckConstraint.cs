using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSaldoCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Cuentas_Saldo_NoNegativo",
                table: "cuenta");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Cuentas_Saldo_NoNegativo",
                table: "cuenta",
                sql: "balance >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Cuentas_Saldo_NoNegativo",
                table: "cuenta");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Cuentas_Saldo_NoNegativo",
                table: "cuenta",
                sql: "balance >= 0");
        }
    }
}
