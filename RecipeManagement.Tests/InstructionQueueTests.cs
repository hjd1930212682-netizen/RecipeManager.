using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class InstructionQueueTests
{
    private static RecipeManager Manager() => new(new[]
    {
        new Recipe { Id = 1, Title = "Soup", Instructions = new() { "Chop", "Boil", "Serve" } },
        new Recipe { Id = 2, Title = "Tea", Instructions = new() { "Steep" } },
        new Recipe { Id = 3, Title = "No steps" }
    });

    [Fact]
    public void EmptyQueueCanBeInspectedAndCompletedSafely()
    {
        var manager = Manager();
        Assert.Equal(0, manager.PendingInstructionCount);
        Assert.Null(manager.PeekNextInstruction());
        Assert.Null(manager.CompleteNextInstruction());
    }

    [Fact]
    public void CookingCompletesInstructionsInOriginalOrder()
    {
        var manager = Manager();
        Assert.True(manager.StartCooking(1));
        Assert.Equal(3, manager.PendingInstructionCount);
        Assert.Equal("Chop", manager.PeekNextInstruction());
        Assert.Equal(3, manager.PendingInstructionCount);
        Assert.Equal("Chop", manager.CompleteNextInstruction());
        Assert.Equal("Boil", manager.CompleteNextInstruction());
        Assert.Equal("Serve", manager.CompleteNextInstruction());
        Assert.Equal(0, manager.PendingInstructionCount);
        Assert.Null(manager.CompleteNextInstruction());
    }

    [Fact]
    public void StartingAnotherRecipeReplacesPendingInstructions()
    {
        var manager = Manager();
        Assert.True(manager.StartCooking(1));
        Assert.True(manager.StartCooking(2));
        Assert.Equal(1, manager.PendingInstructionCount);
        Assert.Equal("Steep", manager.CompleteNextInstruction());
        Assert.Null(manager.PeekNextInstruction());
    }

    [Fact]
    public void InvalidSessionDoesNotDiscardCurrentInstructions()
    {
        var manager = Manager();
        Assert.True(manager.StartCooking(1));
        Assert.False(manager.StartCooking(99));
        Assert.False(manager.StartCooking(3));
        Assert.Equal(3, manager.PendingInstructionCount);
        Assert.Equal("Chop", manager.PeekNextInstruction());
    }
}
