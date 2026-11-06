using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class RemovedHistoryTests
{
    private static RecipeManager Manager() => new(new[]
    {
        new Recipe { Id = 10, Title = "A" },
        new Recipe { Id = 20, Title = "B" }
    });

    [Fact]
    public void EmptyHistoryAndFailedRemovalAreSafe()
    {
        var manager = Manager();
        Assert.Null(manager.PeekLastRemovedRecipe());
        Assert.False(manager.RestoreLastRemovedRecipe());
        Assert.False(manager.RemoveRecipeFromCookingPlan(10));
        Assert.Equal(0, manager.RemovedRecipeCount);
    }

    [Fact]
    public void SuccessfulRemovalsAreRestoredLastInFirstOutAtPlanEnd()
    {
        var manager = Manager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        Assert.True(manager.RemoveRecipeFromCookingPlan(10));
        Assert.True(manager.RemoveRecipeFromCookingPlan(20));
        Assert.Equal(2, manager.RemovedRecipeCount);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.Equal(2, manager.RemovedRecipeCount);
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20, 10 }, manager.GetCookingPlan());
        Assert.Equal(0, manager.RemovedRecipeCount);
    }

    [Fact]
    public void RemovedRecipeThatNoLongerExistsCannotBeRestored()
    {
        var manager = Manager();
        manager.AddRecipeToCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(10);
        Assert.True(manager.RemoveRecipe(10));
        Assert.False(manager.RestoreLastRemovedRecipe());
        Assert.Empty(manager.GetCookingPlan());
        Assert.Null(manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void RecipeAlreadyBackInPlanCannotBeRestoredTwice()
    {
        var manager = Manager();
        manager.AddRecipeToCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.AddRecipeToCookingPlan(10);
        Assert.False(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 10 }, manager.GetCookingPlan());
        Assert.Equal(0, manager.RemovedRecipeCount);
    }
}
