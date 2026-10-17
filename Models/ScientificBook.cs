using System;
using Lab3Books.Models.Interfaces;

namespace Lab3Books.Models;

public class ScientificBook : Book, ICitationCalculable
{
    private string topic = string.Empty;

    public string Topic
    {
        get { return topic; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Тема наукової літератури не може бути порожньою.",
                    nameof(value));
            }

            topic = value;
        }
    }

    public int CitationCount { get; set; }

    public ScientificBook(
        string author,
        int pageCount,
        string language,
        string topic)
        : base(author, pageCount, language)
    {
        Topic = topic;
        CitationCount = 0;
    }

    public double CalculateCitationIndex(int citationCount)
    {
        if (citationCount < 0)
        {
            throw new ArgumentException(
                "Кількість цитувань не може бути від'ємною.",
                nameof(citationCount));
        }

        return (double)citationCount / PageCount;
    }

    public override string Read()
    {
        return $"Читаємо наукову книгу автора {Author} на тему «{Topic}».";
    }

    public override string GetDescription()
    {
        return $"Наукова література. Автор: {Author}; " +
               $"тема: {Topic}; сторінок: {PageCount}; " +
               $"мова: {Language}; цитувань: {CitationCount}.";
    }
}