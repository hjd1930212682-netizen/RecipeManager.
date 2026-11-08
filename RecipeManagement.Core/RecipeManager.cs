using System;
using System.Collections.Generic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    private readonly Dictionary<int, Recipe> _recipes = new();
    private readonly List<string> _shoppingList = new();
    private readonly LinkedList<int> _cookingPlan = new();
    private readonly Stack<int> _removedRecipes = new();
    private readonly Queue<string> _instructions = new();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        ArgumentNullException.ThrowIfNull(recipes);
        foreach (var recipe in recipes)
        {
            if (recipe is null || recipe.Id <= 0 || string.IsNullOrWhiteSpace(recipe.Title)
                || !_recipes.TryAdd(recipe.Id, recipe))
            {
                throw new ArgumentException("Recipes must have unique positive IDs and nonblank titles.", nameof(recipes));
            }
        }
    }

    public int RecipeCount => _recipes.Count;
    public int ShoppingItemCount => _shoppingList.Count;
    public int CookingPlanCount => _cookingPlan.Count;
    public int PendingInstructionCount => _instructions.Count;
    public int RemovedRecipeCount => _removedRecipes.Count;

    public bool AddRecipe(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);
        return recipe.Id > 0 && !string.IsNullOrWhiteSpace(recipe.Title)
            && _recipes.TryAdd(recipe.Id, recipe);
    }

    public Recipe? FindRecipe(int recipeId) =>
        _recipes.TryGetValue(recipeId, out var recipe) ? recipe : null;

    public bool RemoveRecipe(int recipeId) =>
        !_cookingPlan.Contains(recipeId) && _recipes.Remove(recipeId);

    public int AddIngredientsToShoppingList(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out var recipe)) return 0;
        _shoppingList.AddRange(recipe.Ingredients);
        return recipe.Ingredients.Count;
    }

    public IReadOnlyList<string> GetShoppingList() => _shoppingList.ToArray();

    public void ClearShoppingList() => _shoppingList.Clear();

    public bool AddRecipeToCookingPlan(int recipeId)
    {
        if (!_recipes.ContainsKey(recipeId) || _cookingPlan.Contains(recipeId)) return false;
        _cookingPlan.AddLast(recipeId);
        return true;
    }

    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        if (!_cookingPlan.Remove(recipeId)) return false;
        _removedRecipes.Push(recipeId);
        return true;
    }

    public bool RestoreLastRemovedRecipe()
    {
        if (_removedRecipes.Count == 0) return false;
        var recipeId = _removedRecipes.Pop();
        if (!_recipes.ContainsKey(recipeId) || _cookingPlan.Contains(recipeId)) return false;
        _cookingPlan.AddLast(recipeId);
        return true;
    }

    public int? PeekLastRemovedRecipe() =>
        _removedRecipes.Count == 0 ? null : _removedRecipes.Peek();

    public IReadOnlyList<int> GetCookingPlan() => new List<int>(_cookingPlan);

    public bool StartCooking(int recipeId)
    {
        if (!_recipes.TryGetValue(recipeId, out var recipe) || recipe.Instructions.Count == 0) return false;
        _instructions.Clear();
        foreach (var instruction in recipe.Instructions)
        {
            _instructions.Enqueue(instruction);
        }
        return true;
    }

    public string? PeekNextInstruction() =>
        _instructions.Count == 0 ? null : _instructions.Peek();

    public string? CompleteNextInstruction() =>
        _instructions.Count == 0 ? null : _instructions.Dequeue();

    public IReadOnlyList<Recipe> SearchByTitle(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByTitle.");

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByIngredient.");

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count) =>
        throw new NotImplementedException("Part B: implement GetHighestProteinRecipes.");

    public bool AddSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement AddSavedRecipe.");

    public bool RemoveSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement RemoveSavedRecipe.");

    public bool IsRecipeSaved(int recipeId) =>
        throw new NotImplementedException("Part B: implement IsRecipeSaved.");

    public IReadOnlyList<int> GetSavedRecipes() =>
        throw new NotImplementedException("Part B: implement GetSavedRecipes.");
}
