
using System;
using Lab3Books.Models.Interfaces;

namespace Lab3Books.Models;

public abstract class Book : IPrintable, IComparableBook
{
    private int pageCount;

    public string Author { get; set; }

    public int PageCount
    {
        get { return pageCount; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException(
                    "Кількість сторінок повинна бути більшою за нуль.",
                    nameof(value));
            }

            pageCount = value;
        }
    }

    public string Language { get; set; }

    public Book(string author, int pageCount, string language)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException(
                "Автор не може бути порожнім.",
                nameof(author));
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            throw new ArgumentException(
                "Мова книги не може бути порожньою.",
                nameof(language));
        }

        Author = author;
        PageCount = pageCount;
        Language = language;
    }

    public virtual string Read()
    {
        return $"Читаємо книгу автора {Author}.";
    }

    public abstract string GetDescription();

    public string GetInfo()
    {
        return GetDescription();
    }

    public bool IsLongerThan(Book other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return PageCount > other.PageCount;
    }
}
