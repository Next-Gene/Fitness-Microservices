using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCalculationService.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFitnessStat_WeightGoalActivity_WeightGoalActivityId",
                table: "UserFitnessStat");

            migrationBuilder.RenameColumn(
                name: "WeightGoalActivityId",
                table: "UserFitnessStat",
                newName: "weightGoalActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFitnessStat_WeightGoalActivityId",
                table: "UserFitnessStat",
                newName: "IX_UserFitnessStat_weightGoalActivityId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WeightGoalActivity",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFitnessStat_WeightGoalActivity_weightGoalActivityId",
                table: "UserFitnessStat",
                column: "weightGoalActivityId",
                principalTable: "WeightGoalActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFitnessStat_WeightGoalActivity_weightGoalActivityId",
                table: "UserFitnessStat");

            migrationBuilder.RenameColumn(
                name: "weightGoalActivityId",
                table: "UserFitnessStat",
                newName: "WeightGoalActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFitnessStat_weightGoalActivityId",
                table: "UserFitnessStat",
                newName: "IX_UserFitnessStat_WeightGoalActivityId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WeightGoalActivity",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFitnessStat_WeightGoalActivity_WeightGoalActivityId",
                table: "UserFitnessStat",
                column: "WeightGoalActivityId",
                principalTable: "WeightGoalActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
