using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreCard.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserTransactionTable_UpdatedForTransactionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserTransactions");
        }
    }
}
