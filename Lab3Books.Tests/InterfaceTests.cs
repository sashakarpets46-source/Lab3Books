using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab3Books.Models;
using Lab3Books.Models.Interfaces;
using Lab3Books.Models.Services;

namespace Lab3Books.Tests;

[TestClass]
public class InterfaceTests
{
    [TestMethod]
    public void ElectronicBook_ImplementsIPrintable()
    {
        IPrintable book = new ElectronicBook(
            "C# Basics",
            "Іван Петренко",
            5);

        string info = book.GetInfo();

        Assert.IsTrue(info.Contains("C# Basics"));
        Assert.IsTrue(info.Contains("Іван Петренко"));
    }

    [TestMethod]
    public void ScientificBook_ImplementsICitationCalculable()
    {
        ICitationCalculable book = new ScientificBook(
            "Олександр Коваль",
            900,
            "Українська",
            "Інформатика");

        double result = book.CalculateCitationIndex(90);

        Assert.AreEqual(0.1, result, 0.0001);
    }

    [TestMethod]
    public void Book_ImplementsIComparableBook()
    {
        Book first = new ScientificBook(
            "Автор 1", 900, "Українська", "Інформатика");

        Book second = new ScientificBook(
            "Автор 2", 300, "Англійська", "Програмування");

        IComparableBook comparable = first;

        Assert.IsTrue(comparable.IsLongerThan(second));

        Assert.IsFalse(
            ((IComparableBook)second).IsLongerThan(first));
    }

    [TestMethod]
    public void BookPrinter_PrintsDifferentBooks()
    {
        var printer = new BookPrinter();

        IPrintable scientificBook = new ScientificBook(
            "Автор 1", 900, "Українська", "Інформатика");

        IPrintable electronicBook = new ElectronicBook(
            "C# Basics", "Автор 2", 5);

        var result = printer.PrintAll(
            new[] { scientificBook, electronicBook });

        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result[0].Contains("Інформатика"));
        Assert.IsTrue(result[1].Contains("C# Basics"));
    }

    [TestMethod]
    public void ScientificBook_NegativeCitationCount_ThrowsException()
    {
        var book = new ScientificBook(
            "Автор", 900, "Українська", "Інформатика");

        bool exceptionThrown = false;

        try
        {
            book.CalculateCitationIndex(-1);
        }
        catch (ArgumentException)
        {
            exceptionThrown = true;
        }

        Assert.IsTrue(
            exceptionThrown,
            "Має виникнути виняток ArgumentException.");
    }
}
