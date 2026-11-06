using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class CookingPlanTests
{
    private static RecipeManager Manager() => new(new[]
    {
        new Recipe { Id = 10, Title = "A" },
        new Recipe { Id = 20, Title = "B" }
    });

    [Fact]
    public void PlanAppendsExistingRecipesInOrderAndRejectsDuplicates()
    {
        var manager = Manager();
        Assert.False(manager.AddRecipeToCookingPlan(99));
        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.True(manager.AddRecipeToCookingPlan(20));
        Assert.False(manager.AddRecipeToCookingPlan(10));
        Assert.Equal(new[] { 10, 20 }, manager.GetCookingPlan());
        Assert.Equal(2, manager.CookingPlanCount);
    }

    [Fact]
    public void PlannedRecipeCannotBeRemovedFromCatalogue()
    {
        var manager = Manager();
        manager.AddRecipeToCookingPlan(10);
        Assert.False(manager.RemoveRecipe(10));
        Assert.NotNull(manager.FindRecipe(10));
        Assert.True(manager.RemoveRecipe(20));
    }

    [Fact]
    public void ReadingPlanDoesNotExposeInternalLinkedList()
    {
        var manager = Manager();
        manager.AddRecipeToCookingPlan(10);
        var snapshot = manager.GetCookingPlan();
        manager.AddRecipeToCookingPlan(20);
        Assert.Equal(new[] { 10 }, snapshot);
        Assert.Equal(new[] { 10, 20 }, manager.GetCookingPlan());
    }
}
