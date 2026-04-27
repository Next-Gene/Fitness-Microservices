using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionService.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_nutritionFacts_Meals_MealId",
                table: "nutritionFacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_nutritionFacts",
                table: "nutritionFacts");

            migrationBuilder.RenameTable(
                name: "nutritionFacts",
                newName: "NutritionFacts");

            migrationBuilder.RenameIndex(
                name: "IX_nutritionFacts_MealId",
                table: "NutritionFacts",
                newName: "IX_NutritionFacts_MealId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NutritionFacts",
                table: "NutritionFacts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionFacts_Meals_MealId",
                table: "NutritionFacts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionFacts_Meals_MealId",
                table: "NutritionFacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NutritionFacts",
                table: "NutritionFacts");

            migrationBuilder.RenameTable(
                name: "NutritionFacts",
                newName: "nutritionFacts");

            migrationBuilder.RenameIndex(
                name: "IX_NutritionFacts_MealId",
                table: "nutritionFacts",
                newName: "IX_nutritionFacts_MealId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_nutritionFacts",
                table: "nutritionFacts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_nutritionFacts_Meals_MealId",
                table: "nutritionFacts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
