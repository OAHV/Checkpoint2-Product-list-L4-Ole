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

using Checkpoin2_Produc_list_L4_Ole;
using System.Xml.Linq;

bool exit = false;
int cursorLeft = 0;
int cursorTop = 0;

List<Product> products = new List<Product>();       // List of products
List<Category> categories = new List<Category>();   // List of categories
List<Product> found = new List<Product>();          // Products found in search

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
    Console.CursorLeft = 0;
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
    string search = "";
    Console.Clear();
    printProducts();

    Console.CursorTop = 0;
    while ((search = promptInput("Search for product name: ")) == "")
    {
        error("No input. Please try again");
        continue;  // Retry on empty input
    }
    
    found = products.FindAll(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
    printProducts();
}

bool newProduct() 
{
    int price = -1;
    string name = "";

    Console.Clear();
    printProducts();

    Console.CursorTop = 0;
    Console.WriteLine("\nEnter a new product ('q' to quit)");

    // Input Name --------------------------------------------------
    while ((name = promptInput("Name: ")) == "")
    {
        error("No input. Please try again");
        continue;  // Retry on empty input
    }
    if (name == "q") return true;         // Exit on input 'q'

    // Input Price -------------------------------------------------
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

    // Special treatment for category as it is a class --------------
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

    // Add the new product to the list and sort it --------------------
    products.Add(new Product(category, name, price));
    products = products.OrderBy(x => x.Price).ToList();

    return false;  // Do not exit - continue with next product input
}

void highLight(string message)
{
    // Print in highligh colours - for use with table heading and sum
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine(message);
    Console.ResetColor();
}

void error(string message)
{
    // Print error message on faulty input
    Console.SetCursorPosition(0, 0);                // Top of screen
    Console.BackgroundColor = ConsoleColor.Red;     // Warning colors
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(message);                     // Warning message
    Console.ResetColor();
    restoreCursor();                                // Restor cursor to position prior to error
    while (!Console.KeyAvailable) ;                 // Wait for key pressed - to erase warning message from screen
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
    Console.CursorTop = 8;                                                      // Move to below input fields
    Console.WriteLine("Products");
    highLight("Name".PadRight(10) + "Price".PadLeft(10) + "     Category");     // Highlight table header
    foreach (Product p in products)
    {
        // Print all products
        if (found.Contains(p))                      // Mark found products
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        p.print();                                  // Print product
        Console.ResetColor();
    }
    highLight("Sum: $" + products.Sum(x => x.Price));                           // Highlight sum
}

