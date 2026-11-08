# Recipe Management System — Part A

This solution uses the supplied Application, Core model, JSON loader, and test project. `RecipeManagement.Core/RecipeManager.cs` implements the five Part A collections and their methods. The Part B methods remain starter placeholders because Part B is outside this submission.

## Build, test, and run

From this folder, with the .NET 10 SDK installed:

```powershell
dotnet build RecipeManagement.sln
dotnet test RecipeManagement.sln
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

The console application expects an interactive terminal. The supplied menu uses `Console.Clear()` and may fail when its output is redirected to a pipe.

## Project layout

- `RecipeManagement.Core/` — supplied models, loader, interface, and Part A manager implementation.
- `RecipeManagement.Application/` — supplied console menu.
- `RecipeManagement.Tests/` — supplied examples and additional xUnit tests for each collection and an integrated workflow.
- `data/recipes.json` — supplied example data.
