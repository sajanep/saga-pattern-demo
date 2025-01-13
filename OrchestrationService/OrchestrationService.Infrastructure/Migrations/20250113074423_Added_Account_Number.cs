using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrchestrationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Account_Number : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentAccountId",
                table: "OrderSagaState",
                newName: "PaymentAccountNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentAccountNumber",
                table: "OrderSagaState",
                newName: "PaymentAccountId");
        }
    }
}
