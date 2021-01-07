using Microsoft.EntityFrameworkCore.Migrations;

namespace API.GraphQL.Migrations
{
    public partial class Update_CompanyId_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "company_id",
                schema: "ic_internship",
                table: "internships",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "ix_internships_company_id",
                schema: "ic_internship",
                table: "internships",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_internships_companies_company_id",
                schema: "ic_internship",
                table: "internships",
                column: "company_id",
                principalSchema: "ic_company",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_internships_companies_company_id",
                schema: "ic_internship",
                table: "internships");

            migrationBuilder.DropIndex(
                name: "ix_internships_company_id",
                schema: "ic_internship",
                table: "internships");

            migrationBuilder.AlterColumn<long>(
                name: "company_id",
                schema: "ic_internship",
                table: "internships",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
