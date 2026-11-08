using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class PartAWorkflowTests
{
    [Fact]
    public void RecipeMovesThroughCatalogueShoppingPlanHistoryAndCooking()
    {
        var manager = new RecipeManager(Array.Empty<Recipe>());
        var recipe = new Recipe
        {
            Id = 7,
            Title = "Pasta",
            Ingredients = new() { "100 g pasta", "1 tomato" },
            Instructions = new() { "Boil pasta", "Add tomato" }
        };

        Assert.True(manager.AddRecipe(recipe));
        Assert.Equal(2, manager.AddIngredientsToShoppingList(7));
        Assert.Equal(recipe.Ingredients, manager.GetShoppingList());
        Assert.True(manager.AddRecipeToCookingPlan(7));
        Assert.False(manager.RemoveRecipe(7));
        Assert.True(manager.RemoveRecipeFromCookingPlan(7));
        Assert.Equal(7, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 7 }, manager.GetCookingPlan());

        Assert.True(manager.StartCooking(7));
        Assert.Equal("Boil pasta", manager.CompleteNextInstruction());
        Assert.Equal("Add tomato", manager.PeekNextInstruction());
        Assert.Equal(1, manager.PendingInstructionCount);
    }
}
