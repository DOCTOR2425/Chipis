using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chipis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserEntity",
                newName: "Nickname");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "UserEntity",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatEntityId",
                table: "MessageEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "MessageEntity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderUserEntityId",
                table: "MessageEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RefreshTokenEntity",
                columns: table => new
                {
                    RefreshTokenEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenEntity", x => x.RefreshTokenEntityId);
                    table.ForeignKey(
                        name: "FK_RefreshTokenEntity_UserEntity_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntity",
                        principalColumn: "UserEntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_Telephone",
                table: "UserEntity",
                column: "Telephone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntity_ChatEntityId",
                table: "MessageEntity",
                column: "ChatEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntity_SenderUserEntityId",
                table: "MessageEntity",
                column: "SenderUserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenEntity_UserEntityId",
                table: "RefreshTokenEntity",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEntity_ChatEntity_ChatEntityId",
                table: "MessageEntity",
                column: "ChatEntityId",
                principalTable: "ChatEntity",
                principalColumn: "ChatEntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEntity_UserEntity_SenderUserEntityId",
                table: "MessageEntity",
                column: "SenderUserEntityId",
                principalTable: "UserEntity",
                principalColumn: "UserEntityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageEntity_ChatEntity_ChatEntityId",
                table: "MessageEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageEntity_UserEntity_SenderUserEntityId",
                table: "MessageEntity");

            migrationBuilder.DropTable(
                name: "RefreshTokenEntity");

            migrationBuilder.DropIndex(
                name: "IX_UserEntity_Telephone",
                table: "UserEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessageEntity_ChatEntityId",
                table: "MessageEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessageEntity_SenderUserEntityId",
                table: "MessageEntity");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "UserEntity");

            migrationBuilder.DropColumn(
                name: "ChatEntityId",
                table: "MessageEntity");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "MessageEntity");

            migrationBuilder.DropColumn(
                name: "SenderUserEntityId",
                table: "MessageEntity");

            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "UserEntity",
                newName: "Name");
        }
    }
}
