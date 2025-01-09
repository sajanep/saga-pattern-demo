using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrchestrationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderSagaState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    CustomerId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PaymentAccountId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSagaState", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderSagaState");
        }
    }
}
