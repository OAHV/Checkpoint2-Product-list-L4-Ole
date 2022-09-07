// Write a console app which starts by asking the user for:
// 1.Category
// 2.Product Name
// 3.Price
// 
// Your application needs at least 2 classes.
// Use these classes when you add items to a list.
// You should be able to add items to the list(s) until you write "q" (for quit).
// Summarize price when presenting list.
// The list should be sorted from low price to high and a sum at the bottom.
// Make it possible to add more products after presenting the list.
// Add Error handling to your console app.
// Refactor your code using "Linq" if possible.
//
// ##Level 4 (Extra)
// A) Add a Search function making it possible to search for products in presented list
// B) Highlight the searched item in presented list.

bool exit = false;
int cursorLeft = 0;
int cursorTop = 0;

List<Product> products = new List<Product>();       // List of products
List<Category> categories = new List<Category>();   // List of categories

// Clear screen and print header
Console.Clear();
Console.WriteLine("Products application");

while (!exit)   // until input 'q'
{
    printMenu();
    char choise = Console.ReadKey().KeyChar;
    switch (choise)
    {
        case '1':
            while(!newProduct());
            break;
        case '2':
            printProducts();
            break;
        case '3':
            searchProduct();
            break;
        case '4':
            exit = true;
            break;
        default:
            break;
    }
}

void printMenu()
{
    // Print menu
    Console.CursorTop = 0;
    Console.WriteLine("Products program");
    Console.WriteLine("Menu");
    Console.WriteLine("(1) Enter products");
    Console.WriteLine("(2) List products");
    Console.WriteLine("(3) Search products");
    Console.WriteLine("(4) Quit program");
    Console.Write("Choise: ");
}

void searchProduct()
{

}

bool newProduct() 
{
    int price = -1;
    string name = "";

    Console.Clear();
    printProducts();

    Console.CursorTop = 0;
    Console.WriteLine("\nEnter a new product ('q' to quit)");

    while ((name = promptInput("Name: ")) == "")
    {
        error("No input. Please try again");
        continue;  // Retry on empty input
    }
    if (name == "q") return true;         // Exit on input 'q'

    while (price < 0)
    {
        try         // Check for integer input
        {
            price = Convert.ToInt32(promptInput("Price ($): "));
        }
        catch
        {
            error("Not a number. Please try again");
            price = -1;
            continue;
        }
        if (price < 0)   // Check for positiv input
        {
            error("Negative number. Please try again");
            price = -1;
            continue;
        }
    }

    // Special treatment for category as it is a class
    string categoryName = "";
    while ((categoryName = promptInput("Category: ")) == "")
    {
        error("No input");
        continue;           // Retry on empty input
    }
    // Try to find the input category in list
    Category category = categories.Find(x => x.Name == categoryName);
    if (category == null)
    {
        // Create new category if it didn't exist
        category = new Category(categoryName);
        categories.Add(category);
    }

    // Add the new product to the list and sort it
    products.Add(new Product(category, name, price));
    products = products.OrderBy(x => x.Price).ToList();

    return false;
}

void highLight(string message)
{
    // Print table heading and sum in highligh colours
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine(message);
    Console.ResetColor();
}

void error(string message)
{
    Console.SetCursorPosition(0, 0);                // Top of screen
    Console.BackgroundColor = ConsoleColor.Red;     // Warning colors
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(message);                     // Warning message
    Console.ResetColor();
    restoreCursor();                                // Restor cursor to position prior to error
    while (!Console.KeyAvailable) ;                 // Wait for key pressed to erase warning message from screen
    Console.SetCursorPosition(0, 0);                // Erase warning message
    Console.WriteLine("                              ");
    restoreCursor();                                // Restor cursor (again)
}

string promptInput(string prompt = ":")
{
    // Prompt for input
    Console.CursorLeft = 0;         // Set cursor to the left to print prompt
    Console.Write(prompt);
    saveCursor();                   // Save cursor position in case of error
    return Console.ReadLine();      // Return input string
}

void saveCursor()
{
    cursorLeft = Console.CursorLeft;
    cursorTop = Console.CursorTop;
}

void restoreCursor()
{
    Console.CursorTop = cursorTop;
    Console.CursorLeft = cursorLeft;
}

void printProducts()
{
    // Print header and all products on a clear screen, ending with a sum
    Console.Clear();
    Console.CursorTop = 8;
    Console.WriteLine("Products");
    highLight("Name".PadRight(10) + "Price".PadLeft(10) + "     Category");     // Highlight table header
    foreach (Product p in products) p.print();                                  // Print all products
    highLight("Sum: $" + products.Sum(x => x.Price));                           // Highlight sum
}

class Product
{
    public Category partOf { get; set; }    // The product is part of a category
    public string Name { get; set; }
    public int Price { get; set; }

    // Constructor
    public Product(Category partOf, string name, int price)
    {
        this.partOf = partOf;
        Name = name;
        Price = price;
    }

    // Print method
    public void print(int pad = 10)
    {
        Console.WriteLine(Name.PadRight(pad) + (" $" + Price.ToString()).PadLeft(pad) + "     " + partOf.Name);
    }
}

class Category
{
    public Category(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}

