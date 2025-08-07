using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreCard.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserTransactionTable_IndexAddedForUniqueTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTransactions_UserId",
                table: "UserTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_UserId_TransactionDate_Amount_Type",
                table: "UserTransactions",
                columns: new[] { "UserId", "TransactionDate", "Amount", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTransactions_UserId_TransactionDate_Amount_Type",
                table: "UserTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_UserId",
                table: "UserTransactions",
                column: "UserId");
        }
    }
}
