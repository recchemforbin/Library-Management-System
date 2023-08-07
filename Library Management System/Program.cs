using System;
using System.Collections.Generic;
using System.IO;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public Book()
    {
        // Parameterless constructor
    }

    public string ToCsvString()
    {
        return $"{Title},{Author},{IsAvailable}";
    }

    public static Book FromCsvString(string csvLine)
    {
        string[] parts = csvLine.Split(',');
        if (parts.Length >= 3)
        {
            return new Book
            {
                Title = parts[0],
                Author = parts[1],
                IsAvailable = bool.Parse(parts[2])
            };
        }
        else
        {
            // Return a default Book object if there's an issue with the data
            return new Book();
        }
    }
}

public class Library
{
    private List<Book> books = new List<Book>();
    private const string dataFilePath = "library_data.txt";

    public Library()
    {
        if (File.Exists(dataFilePath))
        {
            LoadData();
        }
        else
        {
            // Create the file if it does not exist
            File.Create(dataFilePath).Close();
        }
    }

    public void AddBook(string title, string author)
    {
        Book book = new Book(title, author);
        books.Add(book);
        SaveData();
        Console.WriteLine("Book added successfully!");
    }

    public List<Book> SearchBooks(string query)
    {
        List<Book> foundBooks = books.FindAll(book =>
            book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            book.Author.Contains(query, StringComparison.OrdinalIgnoreCase)
        );

        return foundBooks;
    }

    public void BorrowBook(string title)
    {
        Book book = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null && book.IsAvailable)
        {
            book.IsAvailable = false;
            Console.WriteLine("Book borrowed successfully!");
            SaveData();
        }
        else
        {
            Console.WriteLine("Book not found or already borrowed.");
        }
    }

    public void ReturnBook(string title)
    {
        Book book = books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null && !book.IsAvailable)
        {
            book.IsAvailable = true;
            Console.WriteLine("Book returned successfully!");
            SaveData();
        }
        else
        {
            Console.WriteLine("Book not found or already available.");
        }
    }

    public void DisplayAllBooks()
    {
        Console.WriteLine("Books in the library:");
        foreach (var book in books)
        {
            string availability = book.IsAvailable ? "Available" : "Borrowed";
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Availability: {availability}");
        }
    }

    private void LoadData()
    {
        string[] lines = File.ReadAllLines(dataFilePath);
        foreach (string line in lines)
        {
            Book book = Book.FromCsvString(line);
            books.Add(book);
        }
    }

    private void SaveData()
    {
        List<string> lines = new List<string>();
        foreach (var book in books)
        {
            lines.Add(book.ToCsvString());
        }
        File.WriteAllLines(dataFilePath, lines);
    }
}

public class Program
{
    static void Main()
    {
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("\nWelcome to the Library Management System!");
            Console.WriteLine("1. Add a new book");
            Console.WriteLine("2. Search for books");
            Console.WriteLine("3. Borrow a book");
            Console.WriteLine("4. Return a book");
            Console.WriteLine("5. Display all books");
            Console.WriteLine("6. Exit");

            Console.Write("\nEnter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter the book title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter the book author: ");
                    string author = Console.ReadLine();
                    library.AddBook(title, author);
                    break;
                case 2:
                    Console.Write("Enter the search query: ");
                    string query = Console.ReadLine();
                    List<Book> foundBooks = library.SearchBooks(query);
                    if (foundBooks.Count > 0)
                    {
                        Console.WriteLine("Books found:");
                        foreach (var book in foundBooks)
                        {
                            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, " +
                                              $"Availability: {(book.IsAvailable ? "Available" : "Borrowed")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found matching the search query.");
                    }
                    break;
                case 3:
                    Console.Write("Enter the book title to borrow: ");
                    string borrowTitle = Console.ReadLine();
                    library.BorrowBook(borrowTitle);
                    break;
                case 4:
                    Console.Write("Enter the book title to return: ");
                    string returnTitle = Console.ReadLine();
                    library.ReturnBook(returnTitle);
                    break;
                case 5:
                    library.DisplayAllBooks();
                    break;
                case 6:
                    Console.WriteLine("Exiting the program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
