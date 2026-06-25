# MyBudget Expense Tracker — Assignment 1 (Starter)

Welcome! This starter solution contains everything you need to begin.

```
ExpenseTracker.sln
├─ ExpenseTracker/             the app you will build
│   ├─ BudgetRules.cs          << implement the methods here (start here)
│   └─ Program.cs              << build the console menu UI here
└─ ExpenseTracker.Tests/       provided xUnit tests — DO NOT MODIFY
    └─ BudgetRulesTests.cs
```

## How to work

1. Open `ExpenseTracker.sln` in Visual Studio 2026 (or run from the CLI).
2. Open **Test Explorer** (Test → Test Explorer). Run the tests — they all fail at first.
3. Implement the methods in `BudgetRules.cs` one at a time, re-running the tests until they are all green.
4. Then build the interactive menu UI in `Program.cs` (see the assignment brief for the required features and a sample run).
5. Commit your progress to GitHub as you go.

## Useful commands

```bash
dotnet test            # run the provided unit tests
dotnet run --project ExpenseTracker   # run your console app
```

## Rules

- Do **not** modify `BudgetRulesTests.cs` or change the method signatures in `BudgetRules.cs`. Your code must pass the tests as given.
- Stay within Modules 1–4: no collections, no domain classes, no file I/O. The single provided static class and the custom exception are the only types you need.
