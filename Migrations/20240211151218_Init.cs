using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChat.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    imageUrl = table.Column<string>(type: "text", nullable: false),
                    inviteCode = table.Column<string>(type: "text", nullable: false),
                    profileId = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.id);
                    table.ForeignKey(
                        name: "FK_servers_profiles_profileId",
                        column: x => x.profileId,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    serverId = table.Column<string>(type: "text", nullable: true),
                    profileId = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.id);
                    table.ForeignKey(
                        name: "FK_channels_profiles_profileId",
                        column: x => x.profileId,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channels_servers_serverId",
                        column: x => x.serverId,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    serverId = table.Column<string>(type: "text", nullable: true),
                    profileId = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_members_profiles_profileId",
                        column: x => x.profileId,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_members_servers_serverId",
                        column: x => x.serverId,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    memberOneId = table.Column<string>(type: "text", nullable: false),
                    memberTwoId = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.id);
                    table.ForeignKey(
                        name: "FK_Conversation_members_memberOneId",
                        column: x => x.memberOneId,
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversation_members_memberTwoId",
                        column: x => x.memberTwoId,
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    fileUrl = table.Column<string>(type: "text", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    memberId = table.Column<string>(type: "text", nullable: true),
                    channelId = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.id);
                    table.ForeignKey(
                        name: "FK_Message_channels_channelId",
                        column: x => x.channelId,
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectMessage",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    fileUrl = table.Column<string>(type: "text", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    memberId = table.Column<string>(type: "text", nullable: true),
                    conversationId = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectMessage", x => x.id);
                    table.ForeignKey(
                        name: "FK_DirectMessage_Conversation_conversationId",
                        column: x => x.conversationId,
                        principalTable: "Conversation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectMessage_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "profiles",
                columns: new[] { "id", "createdAt", "email", "imageUrl", "name", "updatedAt" },
                values: new object[] { "df4cc051-d0c4-4b4e-963a-5de8a79859ac", new DateTime(2024, 2, 11, 15, 12, 17, 621, DateTimeKind.Utc).AddTicks(8028), "john@gmail.com", "my image url", "John", null });

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_memberOneId",
                table: "Conversation",
                column: "memberOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_memberTwoId",
                table: "Conversation",
                column: "memberTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessage_conversationId",
                table: "DirectMessage",
                column: "conversationId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessage_memberId",
                table: "DirectMessage",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_channelId",
                table: "Message",
                column: "channelId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_memberId",
                table: "Message",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_channels_profileId",
                table: "channels",
                column: "profileId");

            migrationBuilder.CreateIndex(
                name: "IX_channels_serverId",
                table: "channels",
                column: "serverId");

            migrationBuilder.CreateIndex(
                name: "IX_members_profileId",
                table: "members",
                column: "profileId");

            migrationBuilder.CreateIndex(
                name: "IX_members_serverId",
                table: "members",
                column: "serverId");

            migrationBuilder.CreateIndex(
                name: "IX_servers_profileId",
                table: "servers",
                column: "profileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectMessage");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "servers");

            migrationBuilder.DropTable(
                name: "profiles");
        }
    }
}
