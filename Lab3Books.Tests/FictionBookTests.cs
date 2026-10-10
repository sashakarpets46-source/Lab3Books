
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab3Books.Models;

namespace Lab3Books.Tests;

[TestClass]
public class FictionBookTests
{
    [TestMethod]
    public void Constructor_WithValidData_InitializesCorrectly()
    {
        var book = new FictionBook(
            "Тестовий автор",
            250,
            "Українська",
            "Фантастика",
            5);

        Assert.AreEqual("Тестовий автор", book.Author);
        Assert.AreEqual(250, book.PageCount);
        Assert.AreEqual("Українська", book.Language);
        Assert.AreEqual("Фантастика", book.Genre);
        Assert.AreEqual(5, book.PublicationCount);
    }

    [TestMethod]
    public void Constructor_WithInvalidPageCount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new FictionBook(
                "Тестовий автор",
                0,
                "Українська",
                "Фантастика",
                5));
    }

    [TestMethod]
    public void Constructor_WithEmptyGenre_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new FictionBook(
                "Тестовий автор",
                250,
                "Українська",
                "",
                5));
    }

    [TestMethod]
    public void Constructor_WithNegativePublicationCount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new FictionBook(
                "Тестовий автор",
                250,
                "Українська",
                "Фантастика",
                -1));
    }

    [TestMethod]
    public void GetPublicationCount_ReturnsCorrectValue()
    {
        var book = new FictionBook(
            "Тестовий автор",
            250,
            "Українська",
            "Фантастика",
            5);

        Assert.AreEqual(5, book.GetPublicationCount());
    }

    [TestMethod]
    public void Read_ReturnsDescriptionOfFictionBook()
    {
        var book = new FictionBook(
            "Тестовий автор",
            250,
            "Українська",
            "Фантастика",
            5);

        StringAssert.Contains(book.Read(), "Фантастика");
        StringAssert.Contains(book.Read(), "Тестовий автор");
    }
}
