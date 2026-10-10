
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab3Books.Models;

namespace Lab3Books.Tests;

[TestClass]
public class ScientificBookTests
{
    // Тестуємо успішну ініціалізацію об'єкта з валідними даними.
    [TestMethod]
    public void Constructor_WithValidData_InitializesCorrectly()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        Assert.AreEqual("Тестовий автор", book.Author);
        Assert.AreEqual(250, book.PageCount);
        Assert.AreEqual("Українська", book.Language);
        Assert.AreEqual("Штучний інтелект", book.Topic);
    }

    // Тестуємо конструктор з некоректною кількістю сторінок.
    [TestMethod]
    public void Constructor_WithInvalidPageCount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new ScientificBook(
                "Тестовий автор",
                0,
                "Українська",
                "Штучний інтелект"));
    }

    // Тестуємо валідацію теми наукової книги.
    [TestMethod]
    public void Topic_WithEmptyValue_ThrowsArgumentException()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        Assert.Throws<ArgumentException>(() =>
            book.Topic = "");
    }

    // Тестуємо розрахунок індексу цитування.
    [TestMethod]
    public void CalculateCitationIndex_ReturnsCorrectValue()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        double result = book.CalculateCitationIndex(50);

        Assert.AreEqual(0.2, result, 0.0001);
    }

    // Тестуємо від'ємну кількість цитувань.
    [TestMethod]
    public void CalculateCitationIndex_WithNegativeCitations_ThrowsArgumentException()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        Assert.Throws<ArgumentException>(() =>
            book.CalculateCitationIndex(-1));
    }

    // Тестуємо формування опису наукової книги.
    [TestMethod]
    public void GetDescription_ReturnsCorrectDescription()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        StringAssert.Contains(book.GetDescription(), "Тестовий автор");
        StringAssert.Contains(book.GetDescription(), "Штучний інтелект");
        StringAssert.Contains(book.GetDescription(), "250");
    }

    // Тестуємо перевизначений метод Read.
    [TestMethod]
    public void Read_ReturnsScientificBookInformation()
    {
        var book = new ScientificBook(
            "Тестовий автор",
            250,
            "Українська",
            "Штучний інтелект");

        StringAssert.Contains(book.Read(), "Тестовий автор");
        StringAssert.Contains(book.Read(), "Штучний інтелект");
    }
}
