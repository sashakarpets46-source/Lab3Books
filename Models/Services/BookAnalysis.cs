
using System;
using System.Collections.Generic;
using System.Linq;
using Lab3Books.Models;

namespace Lab3Books.Services;

public class BookAnalysis
{
    public static int GetTotalPagesByLanguage(
        List<Book> books,
        string language)
    {
        if (books == null)
        {
            throw new ArgumentNullException(nameof(books));
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            throw new ArgumentException(
                "Мова не може бути порожньою.",
                nameof(language));
        }

        return books
            .Where(book => book != null &&
                string.Equals(
                    book.Language,
                    language,
                    StringComparison.OrdinalIgnoreCase))
            .Sum(book => book.PageCount);
    }
}
