// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;
using System.Globalization;

using System;
using System.Collections.Generic;
using System.Linq;



namespace MyBudget
{
    class Program
    {

        static decimal monthlyBudget = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  MyBudget Expense Tracker");
            Console.WriteLine("============================================================");

            bool exit = false;

            ;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("1: Add an expense");
                Console.WriteLine("2: View summary");
                Console.WriteLine("3: Set monthly budget");
                Console.WriteLine("4: Exit");
                Console.Write("Please choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddExpense();
                        break;

                    case "2":
                        ViewSummary();
                        break;

                    case "3":
                        SetMonthlyBudget();
                        break;

                    case "4":
                        exit = true;
                        Console.WriteLine("\nThank you for using MyBudget!");
                        break;

                    default:
                        Console.WriteLine("Invalid menu option.");
                        break;
                }
            }


            void AddExpense()
            {
                Console.Write("Description : ");

                string description = "";

                while (true)
                {
                    description = Console.ReadLine() ?? "";
                    if (!string.IsNullOrEmpty(description))
                        break;

                    Console.Write("Description cannot be empty. Enter again: ");
                }

                decimal amount = 0;
                while (true)
                {
                    Console.Write("Amount      : ");
                    if (!decimal.TryParse(Console.ReadLine(), out amount))
                    {
                        Console.WriteLine("Invalid number. Please input again!");
                        continue;
                    }

                    try
                    {
                        var formalAmout = BudgetRules.ValidateAmount(amount);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    break;
                }

                string category = "";

                while (true)
                {
                    Console.Write("Category    : [Food/Transport/Utilities/Entertainment/Other] ");
                    

                    category = BudgetRules.NormalizeCategory(Console.ReadLine());

                    if (!string.IsNullOrEmpty(category))

                      break;
                    

                    Console.WriteLine("  Invalid category.");
                }

                DateTime date;

                while (true)
                {
                    Console.Write("Date (blank = today): ");

                    string input = Console.ReadLine() ?? "";

                    if (string.IsNullOrEmpty(input))
                    {
                        date = DateTime.Today;
                        break;
                    }

                    if (DateTime.TryParseExact(
                        input,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out date))
                    {
                        if (date <= DateTime.Today)
                            break;
                    }

                    Console.WriteLine("  Invalid date.");
                }

                Console.Write("Note (optional): ");

                string? note = Console.ReadLine();

                if (string.IsNullOrEmpty(note))
                    note = null;

                //Expense expense = new Expense
                //{
                //    Description = description,
                //    Amount = amount,
                //    Category = category,
                //    Date = date,
                //    Note = note
                //};

                //expenses.Add(expense);

                Console.WriteLine($"Recorded  : ${amount:F2} | {category} | {date:yyyy-MM-dd}");
                Console.WriteLine($"Size band : {BudgetRules.ClassifyAmount(amount)}");

                if (monthlyBudget > 0)
                {
                    decimal left = monthlyBudget - amount;
                    Console.WriteLine(
                    $"Budget: {BudgetRules.FormatCurrency(left)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} -> {BudgetRules.BudgetStatus(left, monthlyBudget)}");
                } else
                {
                    Console.WriteLine("Please set your monthly budget first");
                }
            }

            static void ViewSummary()
            {
                Console.WriteLine();
                Console.WriteLine("================ Summary ================");

                if (expenses.Count == 0)
                {
                    Console.WriteLine("No expenses recorded.");
                    return;
                }

                double total = expenses.Sum(e => e.Amount);
                double average = total / expenses.Count;
                double highest = expenses.Max(e => e.Amount);

                Console.WriteLine($"Number of expenses : {expenses.Count}");
                Console.WriteLine($"Total spent        : ${total:F2}");
                Console.WriteLine($"Average expense    : ${average:F2}");
                Console.WriteLine($"Highest expense    : ${highest:F2}");

                Console.WriteLine();
                Console.WriteLine("Category totals:");

                foreach (string cat in BudgetRules.ValidCategories)
                {
                    double catTotal = expenses
                        .Where(e => e.Category.Equals(cat, StringComparison.OrdinalIgnoreCase))
                        .Sum(e => e.Amount);

                    Console.WriteLine($"  {cat,-15} ${catTotal:F2}");
                }

                if (monthlyBudget > 0)
                {
                    Console.WriteLine();
                    ShowBudgetStatus();
                }
            }

            static void SetMonthlyBudget()
            {
                double budget;

                while (true)
                {
                    Console.Write("Monthly budget: ");

                    if (double.TryParse(Console.ReadLine(), out budget)
                        && budget > 0)
                        break;

                    Console.WriteLine("  Budget must be greater than zero.");
                }

                monthlyBudget = budget;

                Console.WriteLine($"Budget set to ${monthlyBudget:F2}.");

                ShowBudgetStatus();
            }

            static void ShowBudgetStatus()
            {
                double spent = expenses.Sum(e => e.Amount);
                double remaining = monthlyBudget - spent;

                Console.WriteLine(
                    $"  Budget: ${remaining:F2} remaining of ${monthlyBudget:F2} -> {BudgetRules.BudgetStatus(monthlyBudget, spent)}");
            }
        }

        class Expense
        {
            public string Description { get; set; } = "";

            public double Amount { get; set; }

            public string Category { get; set; } = "";

            public DateTime Date { get; set; }

            public string? Note { get; set; }
        }



        public static readonly string[] ValidCategories =
        {
            "Food",
            "Transport",
            "Utilities",
            "Entertainment",
            "Other"
        };

        public static string ClassifyAmount(double amount)
        {
            if (amount < 20)
                return "Small";

            if (amount < 100)
                return "Medium";

            return "Large";
        }

        public static string BudgetStatus(double budget, double spent)
        {
            if (spent > budget)
                return "Over budget";

            double remaining = budget - spent;

            if (remaining < budget * 0.10)
                return "Almost out";

            return "On track";
        }
    }
}

// Delete the line above and implement the application here.
