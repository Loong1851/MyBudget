// =====================================================================
//  BudgetRulesTests.cs  —  the instructor-provided self-check suite.
//  DO NOT MODIFY these tests to match your code. Implement BudgetRules so
//  that every test below passes (Test Explorer, or: dotnet test).
//
//  These tests demonstrate the Module 9 xUnit features you will study later:
//  [Fact], [Theory] / [InlineData], and the Arrange-Act-Assert pattern.
// =====================================================================
using ExpenseTracker;
using Xunit;

namespace ExpenseTracker.Tests;

public class BudgetRulesTests
{
    // ----- ValidateAmount ------------------------------------------------
    [Theory]
    [InlineData(0.01)]
    [InlineData(10)]
    [InlineData(999999.99)]
    public void ValidateAmount_AcceptsPositiveAmounts(decimal amount)
    {
        decimal result = BudgetRules.ValidateAmount(amount);
        Assert.Equal(amount, result);
    }

    [Fact]
    public void ValidateAmount_RoundsToTwoDecimalPlaces()
    {
        // Arrange / Act
        decimal result = BudgetRules.ValidateAmount(10.999m);
        // Assert
        Assert.Equal(11.00m, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void ValidateAmount_RejectsNonPositive(decimal amount)
    {
        Assert.Throws<InvalidExpenseException>(() => BudgetRules.ValidateAmount(amount));
    }

    [Fact]
    public void ValidateAmount_RejectsUnrealisticallyLargeAmount()
    {
        Assert.Throws<InvalidExpenseException>(() => BudgetRules.ValidateAmount(2_000_000m));
    }

    // ----- ClassifyAmount ------------------------------------------------
    [Theory]
    [InlineData(5, "Micro")]
    [InlineData(9.99, "Micro")]
    [InlineData(10, "Small")]
    [InlineData(49.99, "Small")]
    [InlineData(50, "Medium")]
    [InlineData(199.99, "Medium")]
    [InlineData(200, "Large")]
    [InlineData(5000, "Large")]
    public void ClassifyAmount_ReturnsExpectedBand(decimal amount, string expected)
    {
        Assert.Equal(expected, BudgetRules.ClassifyAmount(amount));
    }

    [Fact]
    public void ClassifyAmount_RejectsNonPositive()
    {
        Assert.Throws<InvalidExpenseException>(() => BudgetRules.ClassifyAmount(0m));
    }

    // ----- NormalizeCategory --------------------------------------------
    [Theory]
    [InlineData("food", "Food")]
    [InlineData("FOOD", "Food")]
    [InlineData("  Transport ", "Transport")]
    [InlineData("u", "Utilities")]
    [InlineData("Entertainment", "Entertainment")]
    [InlineData("o", "Other")]
    public void NormalizeCategory_MapsKnownValues(string input, string expected)
    {
        Assert.Equal(expected, BudgetRules.NormalizeCategory(input));
    }

    [Theory]
    [InlineData("groceries")]
    [InlineData("")]
    [InlineData(null)]
    public void NormalizeCategory_ReturnsNullForUnknown(string? input)
    {
        Assert.Null(BudgetRules.NormalizeCategory(input));
    }

    // ----- BudgetStatus --------------------------------------------------
    [Theory]
    [InlineData(-1, 500, "OVER BUDGET")]
    [InlineData(0, 500, "Almost out")]
    [InlineData(49, 500, "Almost out")]   // 10% of 500 is 50
    [InlineData(50, 500, "On track")]
    [InlineData(500, 500, "On track")]
    public void BudgetStatus_ReturnsExpectedMessage(decimal remaining, decimal limit, string expected)
    {
        Assert.Equal(expected, BudgetRules.BudgetStatus(remaining, limit));
    }

    [Fact]
    public void BudgetStatus_RejectsNonPositiveLimit()
    {
        Assert.Throws<InvalidExpenseException>(() => BudgetRules.BudgetStatus(10m, 0m));
    }

    // ----- FormatCurrency (and overload) --------------------------------
    [Theory]
    [InlineData(62.4, "$62.40")]
    [InlineData(5, "$5.00")]
    [InlineData(1000.5, "$1000.50")]
    public void FormatCurrency_UsesDefaultDollarSymbol(decimal amount, string expected)
    {
        Assert.Equal(expected, BudgetRules.FormatCurrency(amount));
    }

    [Fact]
    public void FormatCurrency_AcceptsCustomSymbol()
    {
        Assert.Equal("£10.00", BudgetRules.FormatCurrency(10m, "£"));
    }
}
