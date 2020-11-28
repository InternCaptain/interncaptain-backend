using Microsoft.EntityFrameworkCore.Migrations;

namespace API.GraphQL.Migrations
{
    public partial class UserProfilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profile_picture",
                schema: "ic_user",
                table: "users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_picture",
                schema: "ic_user",
                table: "users");
        }
    }
}
