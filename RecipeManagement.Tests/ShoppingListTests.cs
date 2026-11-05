using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class ShoppingListTests
{
    private static RecipeManager CreateManager() => new(new[]
    {
        new Recipe
        {
            Id = 1,
            Title = "Pancakes",
            Ingredients = new() { "2 eggs", "1 cup flour" }
        },
        new Recipe
        {
            Id = 2,
            Title = "Tea",
            Ingredients = new() { "water", "tea leaves" }
        }
    });

    [Fact]
    public void IngredientsAreAppendedInRecipeOrder()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.AddIngredientsToShoppingList(1));
        Assert.Equal(2, manager.AddIngredientsToShoppingList(2));
        Assert.Equal(new[] { "2 eggs", "1 cup flour", "water", "tea leaves" }, manager.GetShoppingList());
        Assert.Equal(4, manager.ShoppingItemCount);
    }

    [Fact]
    public void MissingRecipeDoesNotChangeTheList()
    {
        var manager = CreateManager();
        Assert.Equal(0, manager.AddIngredientsToShoppingList(999));
        Assert.Empty(manager.GetShoppingList());
    }

    [Fact]
    public void ReadingReturnsSnapshotAndClearRemovesItems()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(1);
        var snapshot = manager.GetShoppingList();
        manager.ClearShoppingList();
        Assert.Equal(new[] { "2 eggs", "1 cup flour" }, snapshot);
        Assert.Empty(manager.GetShoppingList());
        Assert.Equal(0, manager.ShoppingItemCount);
    }
}
