
using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Lab3Books.Models;
using Lab3Books.Services;

namespace Lab3Books;

public partial class MainWindow : Window
{
    private readonly List<Book> books = new();

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

            BooksListBox.Items.Add(
                book.GetDescription() + extraInfo);

            BookCountTextBlock.Text =
                $"Кількість книг: {books.Count}";

            AuthorTextBox.Text = "";
            PageCountTextBox.Text = "";
            GenreTextBox.Text = "";
            TopicTextBox.Text = "";
            PublicationCountTextBox.Text = "";
            CitationCountTextBox.Text = "";

            AnalysisResultTextBlock.Text =
                "Книгу успішно додано.";
        }
        catch (Exception ex)
        {
            AnalysisResultTextBlock.Text =
                $"Помилка: {ex.Message}";
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

        AnalysisResultTextBlock.Text =
            books[selectedIndex].Read();
    }

    private void AnalyzeButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        try
        {
            string language =
                AnalysisLanguageTextBox.Text?.Trim() ?? "";

            int totalPages =
                BookAnalysis.GetTotalPagesByLanguage(
                    books,
                    language);

            AnalysisResultTextBlock.Text =
                $"Загальна кількість сторінок мовою " +
                $"«{language}»: {totalPages}.";
        }
        catch (Exception ex)
        {
            AnalysisResultTextBlock.Text =
                $"Помилка: {ex.Message}";
        }
    }
}
