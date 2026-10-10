using System;

namespace Lab3Books.Models;

public class FictionBook : Book
{
    public string Genre { get; set; }

    public int PublicationCount { get; set; }

    public FictionBook(
        string author,
        int pageCount,
        string language,
        string genre,
        int publicationCount)
        : base(author, pageCount, language)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentException(
                "Жанр не може бути порожнім.", nameof(genre));
        }

        if (publicationCount < 0)
        {
            throw new ArgumentException(
                "Кількість видань не може бути від'ємною.",
                nameof(publicationCount));
        }

        Genre = genre;
        PublicationCount = publicationCount;
    }

    public override string Read()
    {
        return $"Читаємо художню книгу жанру «{Genre}» автора {Author}.";
    }

    public override string GetDescription()
    {
        return $"Художня література. Автор: {Author}; " +
               $"жанр: {Genre}; сторінок: {PageCount}; " +
               $"мова: {Language}; кількість видань: {PublicationCount}.";
    }

    public int GetPublicationCount()
    {
        return PublicationCount;
    }
}
