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

    public string ToCsvString()
    {
        return $"{Title},{Author},{IsAvailable}";
    }

    public static Book FromCsvString(string csvLine)
    {
        string[] parts = csvLine.Split(',');
        return new Book
        {
            Title = parts[0],
            Author = parts[1],
            IsAvailable = bool.Parse(parts[2])
        };
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

    // Other methods remain the same

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
                // Other cases remain the same
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
