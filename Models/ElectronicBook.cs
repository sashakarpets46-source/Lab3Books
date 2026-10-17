using System;
using Lab3Books.Models.Interfaces;

namespace Lab3Books.Models;

public class ElectronicBook : IPrintable
{
    public string Title { get; set; }

    public string Author { get; set; }

    public int FileSizeMB { get; set; }

    public ElectronicBook(string title, string author, int fileSizeMB)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Назва електронної книги не може бути порожньою.",
                nameof(title));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException(
                "Автор не може бути порожнім.",
                nameof(author));
        }

        if (fileSizeMB <= 0)
        {
            throw new ArgumentException(
                "Розмір файлу повинен бути більшим за нуль.",
                nameof(fileSizeMB));
        }

        Title = title;
        Author = author;
        FileSizeMB = fileSizeMB;
    }

    public string GetInfo()
    {
        return $"Електронна книга: {Title}; " +
               $"автор: {Author}; " +
               $"розмір файлу: {FileSizeMB} МБ.";
    }
}
