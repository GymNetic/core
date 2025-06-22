using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GYMNETIC.Core.Migrations
{
    /// <inheritdoc />
    public partial class ueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_NutriPlan_NutriPlanId1",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "NutriPlanId1",
                table: "Customer",
                newName: "NutriPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_NutriPlanId1",
                table: "Customer",
                newName: "IX_Customer_NutriPlanId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MonthlyPlans",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "MonthlyPlanId",
                table: "Customer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyPlans",
                table: "MonthlyPlans",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MonthlyPlanId",
                table: "Customer",
                column: "MonthlyPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_MonthlyPlans_MonthlyPlanId",
                table: "Customer",
                column: "MonthlyPlanId",
                principalTable: "MonthlyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_NutriPlan_NutriPlanId",
                table: "Customer",
                column: "NutriPlanId",
                principalTable: "NutriPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_MonthlyPlans_MonthlyPlanId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_NutriPlan_NutriPlanId",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyPlans",
                table: "MonthlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MonthlyPlanId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MonthlyPlans");

            migrationBuilder.DropColumn(
                name: "MonthlyPlanId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "NutriPlanId",
                table: "Customer",
                newName: "NutriPlanId1");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_NutriPlanId",
                table: "Customer",
                newName: "IX_Customer_NutriPlanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_NutriPlan_NutriPlanId1",
                table: "Customer",
                column: "NutriPlanId1",
                principalTable: "NutriPlan",
                principalColumn: "Id");
        }
    }
}
