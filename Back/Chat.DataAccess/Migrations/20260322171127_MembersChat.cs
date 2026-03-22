using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chipis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MembersChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "MessageEntity",
                newName: "SentAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "MessageEntity",
                newName: "Date");
        }
    }
}
