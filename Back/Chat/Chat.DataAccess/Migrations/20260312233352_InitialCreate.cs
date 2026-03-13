using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chipis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatEntity",
                columns: table => new
                {
                    ChatEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEntity", x => x.ChatEntityId);
                });

            migrationBuilder.CreateTable(
                name: "MessageEntity",
                columns: table => new
                {
                    MessageEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEntity", x => x.MessageEntityId);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    UserEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.UserEntityId);
                });

            migrationBuilder.CreateTable(
                name: "ChatMemberEntity",
                columns: table => new
                {
                    ChatMemberEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMemberEntity", x => x.ChatMemberEntityId);
                    table.ForeignKey(
                        name: "FK_ChatMemberEntity_ChatEntity_ChatEntityId",
                        column: x => x.ChatEntityId,
                        principalTable: "ChatEntity",
                        principalColumn: "ChatEntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMemberEntity_UserEntity_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntity",
                        principalColumn: "UserEntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMemberEntity_ChatEntityId",
                table: "ChatMemberEntity",
                column: "ChatEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMemberEntity_UserEntityId",
                table: "ChatMemberEntity",
                column: "UserEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMemberEntity");

            migrationBuilder.DropTable(
                name: "MessageEntity");

            migrationBuilder.DropTable(
                name: "ChatEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");
        }
    }
}
