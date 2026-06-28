// =====================================================================
//  BudgetRules.cs  —  the PROVIDED, TESTABLE CONTRACT for Assignment 1.
//
//  >>> THIS IS WHERE YOU WORK. <<<
//
//  The instructor-provided xUnit tests (ExpenseTracker.Tests) target this
//  class by name. Do NOT rename the class, the methods, or change their
//  signatures, and do NOT modify the tests. Replace each "throw new
//  NotImplementedException()" with a correct implementation so that every
//  test passes (Test Explorer, or:  dotnet test).
//
//  Work one method at a time. Run the tests, watch them turn green.
//  All methods are pure: no Console I/O, no shared state.
// =====================================================================
namespace ExpenseTracker;

public static class BudgetRules
{
    public const decimal MaxAmount = 1_000_000m;     // upper sanity limit
    public const decimal NearLimitFraction = 0.10m;  // "almost out" threshold

    /// <summary>
    /// Validates a monetary amount and returns it rounded to two decimal places.
    /// Throw <see cref="InvalidExpenseException"/> when the amount is not greater
    /// than zero, or is greater than <see cref="MaxAmount"/>.
    /// </summary>
    public static decimal ValidateAmount(decimal amount)
    {
        // TODO: guard clauses + decimal.Round(amount, 2)
        if (amount <= MaxAmount && amount > 0)
        {
            return Math.Round(amount, 2);
        } 
        throw new InvalidExpenseException("Your amount is not right");

    }

    /// <summary>
    /// Classifies a positive amount into a size band: "Micro" (&lt; 10),
    /// "Small" (&lt; 50), "Medium" (&lt; 200), otherwise "Large".
    /// Throw <see cref="InvalidExpenseException"/> for a non-positive amount.
    /// Hint: use a switch expression with relational patterns.
    /// </summary>
    public static string ClassifyAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidExpenseException("amount is not right");
        }

        string result = amount switch
        {
            < 10 => "Micro",
            < 50 => "Small",
            < 200 => "Medium",
            _ => "Large",
        };
        return result;
    }

    /// <summary>
    /// Maps free-text input to one of the five canonical category names
    /// (Food, Transport, Utilities, Entertainment, Other), case-insensitively
    /// and allowing the first-letter shortcut. Return <c>null</c> when the
    /// input is not recognised. Hint: a switch expression over the trimmed,
    /// lower-cased input, with "food" or "f" => "Food", etc.
    /// </summary>
    public static string? NormalizeCategory(string? input)
    {
        var data = (input ?? "nothing").ToLower().Trim();
        string? result = data switch
        {
            "food" => "Food",
            "f" => "Food",
            "transport" => "Transport",
            "t" => "Transport",
            "utilities" => "Utilities",
            "u" => "Utilities",
            "entertainment" => "Entertainment",
            "e" => "Entertainment",
            "other" => "Other",
            "o" => "Other",
            _ => null,

        };
        return result;
    }

    /// <summary>
    /// Returns a budget-status message from the remaining funds and the limit:
    /// "OVER BUDGET" (remaining &lt; 0), "Almost out" (less than 10% of the
    /// limit remains), or "On track". Throw when the limit is not positive.
    /// </summary>
    public static string BudgetStatus(decimal remaining, decimal monthlyLimit)
    {
        if (monthlyLimit <= 0)
        {
            throw new InvalidExpenseException("monthlyLimit is not negative");
        }
        if (remaining < 0)
        {
            return "OVER BUDGET";
        }
        else if (remaining < monthlyLimit * NearLimitFraction)
        {
            return "Almost out";
        }
        else
        {
            return "On track";
        }


    }

    /// <summary>
    /// Formats an amount as currency using the default "$" symbol.
    /// Implement this as an expression-bodied member that calls the overload.
    /// </summary>
    public static string FormatCurrency(decimal amount)
    {
        // TODO: return FormatCurrency(amount, "$");
        string result = $"${amount:F2}";
        return result;
    }

    /// <summary>
    /// Formats an amount as currency using the given symbol, e.g. "$62.40".
    /// </summary>
    public static string FormatCurrency(decimal amount, string currencySymbol)
    {
        string result = $"{currencySymbol}{amount:F2}";
        return result;  
    }
}

/// <summary>
/// Custom exception for invalid expense data (Module 4: custom exceptions).
/// This one is provided complete — use it from the methods above.
/// </summary>
public sealed class InvalidExpenseException(string message) : Exception(message);
