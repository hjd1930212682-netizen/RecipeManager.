using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class CatalogueTests
{
    private static Recipe Recipe(int id, string title = "Soup") => new() { Id = id, Title = title };

    [Fact]
    public void ConstructorBuildsDictionaryAndRejectsInvalidEntries()
    {
        Assert.Throws<ArgumentNullException>(() => new RecipeManager(null!));
        Assert.Throws<ArgumentException>(() => new RecipeManager(new[] { Recipe(1), Recipe(1) }));
        Assert.Throws<ArgumentException>(() => new RecipeManager(new[] { Recipe(0) }));
        Assert.Throws<ArgumentException>(() => new RecipeManager(new[] { Recipe(1, " ") }));

        var manager = new RecipeManager(new[] { Recipe(4), Recipe(9) });
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal(9, manager.FindRecipe(9)?.Id);
        Assert.Null(manager.FindRecipe(99));
    }

    [Fact]
    public void AddRejectsNullInvalidAndDuplicateRecipes()
    {
        var manager = new RecipeManager(Array.Empty<Recipe>());
        Assert.Throws<ArgumentNullException>(() => manager.AddRecipe(null!));
        Assert.False(manager.AddRecipe(Recipe(-1)));
        Assert.False(manager.AddRecipe(Recipe(1, " ")));
        Assert.True(manager.AddRecipe(Recipe(1)));
        Assert.False(manager.AddRecipe(Recipe(1, "Other")));
        Assert.Equal("Soup", manager.FindRecipe(1)?.Title);
        Assert.Equal(1, manager.RecipeCount);
    }

    [Fact]
    public void RemoveUpdatesCatalogueAndMissingIdIsSafe()
    {
        var manager = new RecipeManager(new[] { Recipe(1) });
        Assert.False(manager.RemoveRecipe(99));
        Assert.True(manager.RemoveRecipe(1));
        Assert.Null(manager.FindRecipe(1));
        Assert.Equal(0, manager.RecipeCount);
        Assert.False(manager.RemoveRecipe(1));
    }
}
