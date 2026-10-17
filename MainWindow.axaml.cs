using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Lab3Books.Models;
using Lab3Books.Models.Interfaces;
using Lab3Books.Models.Services;
using Lab3Books.Services;

namespace Lab3Books;

public partial class MainWindow : Window
{
    private readonly List<Book> books = new();
    private readonly BookPrinter bookPrinter = new();

    private bool isInitializing = true;

    public MainWindow()
    {
        InitializeComponent();

        bool isFiction = BookTypeComboBox.SelectedIndex == 0;

        FictionFieldsPanel.IsVisible = isFiction;
        ScientificFieldsPanel.IsVisible = !isFiction;

        isInitializing = false;
    }

    private void BookTypeComboBox_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (isInitializing)
            return;

        bool isFiction = BookTypeComboBox.SelectedIndex == 0;

        FictionFieldsPanel.IsVisible = isFiction;
        ScientificFieldsPanel.IsVisible = !isFiction;

        GenreTextBox.Text = "";
        PublicationCountTextBox.Text = "";
        TopicTextBox.Text = "";
        CitationCountTextBox.Text = "";

        AnalysisResultTextBlock.Text =
            isFiction
                ? "Вибрано тип: художня книга."
                : "Вибрано тип: наукова книга.";
    }

    private void AddBookButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        try
        {
            string author = AuthorTextBox.Text?.Trim() ?? "";
            string language = LanguageTextBox.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Введіть автора книги.");

            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentException("Введіть мову книги.");

            if (!int.TryParse(PageCountTextBox.Text, out int pageCount)
                || pageCount <= 0)
            {
                throw new ArgumentException(
                    "Кількість сторінок має бути додатним числом.");
            }

            Book book;
            string extraInfo = "";

            if (BookTypeComboBox.SelectedIndex == 0)
            {
                string genre = GenreTextBox.Text?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(genre))
                    throw new ArgumentException(
                        "Введіть жанр художньої книги.");

                if (!int.TryParse(
                    PublicationCountTextBox.Text,
                    out int publicationCount)
                    || publicationCount < 0)
                {
                    throw new ArgumentException(
                        "Введіть коректну кількість видань.");
                }

                book = new FictionBook(
                    author,
                    pageCount,
                    language,
                    genre,
                    publicationCount);
            }
            else if (BookTypeComboBox.SelectedIndex == 1)
            {
                string topic = TopicTextBox.Text?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(topic))
                    throw new ArgumentException(
                        "Введіть тему наукової книги.");

                if (!int.TryParse(
                    CitationCountTextBox.Text,
                    out int citationCount)
                    || citationCount < 0)
                {
                    throw new ArgumentException(
                        "Введіть коректну кількість цитувань.");
                }

                var scientificBook = new ScientificBook(
                    author,
                    pageCount,
                    language,
                    topic);

                scientificBook.CitationCount = citationCount;

                double citationIndex =
                    scientificBook.CalculateCitationIndex(citationCount);

                book = scientificBook;

                extraInfo = $"\nІндекс цитування: {citationIndex:F2}";
            }
            else
            {
                throw new ArgumentException("Оберіть тип книги.");
            }

            books.Add(book);
            BooksListBox.Items.Add(book.GetDescription() + extraInfo);

            BookCountTextBlock.Text = $"Кількість книг: {books.Count}";

            AuthorTextBox.Text = "";
            PageCountTextBox.Text = "";
            GenreTextBox.Text = "";
            TopicTextBox.Text = "";
            PublicationCountTextBox.Text = "";
            CitationCountTextBox.Text = "";

            AnalysisResultTextBlock.Text = "Книгу успішно додано.";
        }
        catch (Exception ex)
        {
            AnalysisResultTextBlock.Text = $"Помилка: {ex.Message}";
        }
    }

    private void ReadBookButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        int selectedIndex = BooksListBox.SelectedIndex;

        if (selectedIndex < 0 || selectedIndex >= books.Count)
        {
            AnalysisResultTextBlock.Text =
                "Спочатку виберіть книгу зі списку.";
            return;
        }

        AnalysisResultTextBlock.Text = books[selectedIndex].Read();
    }

    private void AnalyzeButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        try
        {
            string language = AnalysisLanguageTextBox.Text?.Trim() ?? "";

            int totalPages =
                BookAnalysis.GetTotalPagesByLanguage(books, language);

            AnalysisResultTextBlock.Text =
                $"Загальна кількість сторінок мовою «{language}»: " +
                $"{totalPages}.";
        }
        catch (Exception ex)
        {
            AnalysisResultTextBlock.Text = $"Помилка: {ex.Message}";
        }
    }

    private void FindMaxCitationsButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        ScientificBook? bestBook =
            BookAnalysis.GetBookWithMaxCitations(books);

        if (bestBook == null)
        {
            AnalysisResultTextBlock.Text =
                "У списку немає наукових книг.";
            return;
        }

        AnalysisResultTextBlock.Text =
            "Наукова книга з найбільшою кількістю цитувань:\n" +
            $"Автор: {bestBook.Author}\n" +
            $"Тема: {bestBook.Topic}\n" +
            $"Кількість сторінок: {bestBook.PageCount}\n" +
            $"Мова: {bestBook.Language}\n" +
            $"Кількість цитувань: {bestBook.CitationCount}";
    }

    // IPrintable: отримання інформації через інтерфейс.
    private void PrintBookInfoButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        int index = BooksListBox.SelectedIndex;

        if (index < 0 || index >= books.Count)
        {
            InterfaceInfoTextBlock.Text =
                "Спочатку виберіть книгу зі списку.";
            return;
        }

        IPrintable printable = books[index];
        InterfaceInfoTextBlock.Text = bookPrinter.PrintInfo(printable);
    }

    // ICitationCalculable: обчислення індексу цитування.
    private void CalculateCitationButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        int index = BooksListBox.SelectedIndex;

        if (index < 0 || index >= books.Count)
        {
            CitationResultTextBlock.Text =
                "Спочатку виберіть наукову книгу зі списку.";
            return;
        }

        if (books[index] is not ScientificBook scientificBook)
        {
            CitationResultTextBlock.Text =
                "Вибрана книга не є науковою.";
            return;
        }

        if (!int.TryParse(
            DemoCitationCountTextBox.Text,
            out int citationCount)
            || citationCount < 0)
        {
            CitationResultTextBlock.Text =
                "Введіть невід'ємну кількість цитувань.";
            return;
        }

        ICitationCalculable calculable = scientificBook;
        double result = calculable.CalculateCitationIndex(citationCount);

        CitationResultTextBlock.Text = $"Індекс цитування: {result:F3}";
    }

    // IComparableBook: порівняння книг за кількістю сторінок.
    private void CompareBooksButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        int index = BooksListBox.SelectedIndex;

        if (index < 0 || index >= books.Count)
        {
            CompareResultTextBlock.Text =
                "Спочатку виберіть книгу зі списку.";
            return;
        }

        Book selectedBook = books[index];

        Book? otherBook = books
            .Where((book, i) => i != index)
            .FirstOrDefault();

        if (otherBook == null)
        {
            CompareResultTextBlock.Text =
                "Додайте ще одну книгу для порівняння.";
            return;
        }

        IComparableBook comparable = selectedBook;
        bool isLonger = comparable.IsLongerThan(otherBook);

        if (selectedBook.PageCount == otherBook.PageCount)
        {
            CompareResultTextBlock.Text =
                $"Книги мають однакову кількість сторінок: " +
                $"{selectedBook.PageCount}.";
        }
        else
        {
            string author = isLonger
                ? selectedBook.Author
                : otherBook.Author;

            int pages = Math.Max(
                selectedBook.PageCount,
                otherBook.PageCount);

            CompareResultTextBlock.Text =
                $"Більше сторінок має книга автора «{author}»: {pages}.";
        }
    }

    // Створення електронної книги та демонстрація IPrintable.
    private void CreateElectronicBookButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        try
        {
            string title = ElectronicTitleTextBox.Text?.Trim() ?? "";
            string author = ElectronicAuthorTextBox.Text?.Trim() ?? "";

            if (!int.TryParse(
                ElectronicFileSizeTextBox.Text,
                out int fileSizeMB)
                || fileSizeMB <= 0)
            {
                throw new ArgumentException(
                    "Розмір файлу має бути додатним числом.");
            }

            var electronicBook = new ElectronicBook(
                title,
                author,
                fileSizeMB);

            IPrintable printable = electronicBook;

            ElectronicBookResultTextBlock.Text =
                bookPrinter.PrintInfo(printable);
        }
        catch (Exception ex)
        {
            ElectronicBookResultTextBlock.Text =
                $"Помилка: {ex.Message}";
        }
    }
}
