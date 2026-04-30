using System;
using System.Net;
class Product
{
    public int Id;
    public string Name = "" ;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }
    public bool HasEnoughStock(int quantity)
    {
        return RemainingStock >= quantity;
    }
    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}   
class Program
{
    static void Main()
    {
        Product[] products =
        {
            new Product { Id = 101, Name = "iphone", Price = 10000, RemainingStock = 5},
            new Product { Id = 102, Name = "samsung", Price = 8000, RemainingStock = 20},
            new Product { Id = 103, Name = "vivo",Price = 4000, RemainingStock = 10}
        };
        int[] cartId = new int[10];
        int[] cartQty = new int[10];
        double[] cartTotal = new double[10];
        int cartCount = 0;
 
        char choice = 'Y';

        while (choice == 'Y' || choice == 'y' )
        {
            Console.WriteLine("\n=== STORE MENU ===");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}. {p.Name} - ₱{p.Price} (Stock: {p.RemainingStock})");
            }
            Console.Write("\nEnter product ID: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }
            Product? selected = null;
            foreach (var p in products)
            {
                if (p.Id == id)
                    selected = p;
            }
            if (selected == null)
            {
                Console.WriteLine("Product not found");
                continue;
            }
            Console.Write("Enter quantity: ");
            int qty;
            if(!int.TryParse(Console.ReadLine(), out qty)  || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }
            if (qty > selected.RemainingStock)
            {
                Console.WriteLine("Not enough stock.");
                continue;
            }
            bool found = false;
            for (int i = 0; i < cartCount; i++)
            {
                 if (cartId[i] == selected.Id)
                {
                    cartQty[i] += qty;
                    cartTotal[i] = cartQty[i] * selected.Price;
                    found = true;
                    break;
                }
            } 
            if (!found)
            {
                if (cartCount >= 10)
                {
                    Console.WriteLine("Cart is full. Cannot add more unique items.");
                }
                else
                {
                    cartId[cartCount] = selected.Id;
                    cartQty[cartCount] = qty;
                    cartTotal[cartCount] = qty * selected.Price;
                    cartCount++;
                }
                
            }  
            selected.RemainingStock -= qty;
            Console.WriteLine("Added to cart!");
            Console.Write("\nAdd more? (Y/N): ");
            choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
        }
        Console.WriteLine("\n\n=== RECEIPT ===");
        double grandTotal = 0;
        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine($"Item {cartId[i]} x{cartQty[i]} = ₱{cartTotal[i]}");
            grandTotal += cartTotal[i];
        }
        Console.WriteLine($"Grand Total: ₱{grandTotal}");
        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
        }
        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Discount: ₱{discount}");
        Console.WriteLine($"Final Total: ₱{finalTotal}");
        Console.WriteLine("\n=== UPDATED STOCK ===");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Name}: {p.RemainingStock}");
        }
    }
}