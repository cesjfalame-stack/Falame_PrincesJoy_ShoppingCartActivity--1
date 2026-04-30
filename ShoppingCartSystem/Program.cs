using System;
using System.Net;
class Product
{
    public int Id;
    public string Name = "" ;
    public string Category = "";
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
      public void AddStock(int quantity)
    {
        RemainingStock += quantity;
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }
}   
class Program
{
    static void Main()
    {   
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Product[] products =
        {
            new Product { Id = 101, Name = "iphone", Category = "Electronics", Price = 10000, RemainingStock = 5},
            new Product { Id = 102, Name = "samsung", Category = "Electronics", Price = 8000, RemainingStock = 20},
            new Product { Id = 103, Name = "vivo", Category = "Electronics", Price = 4000, RemainingStock = 10},

            new Product { Id = 201, Name = "smart watch", Category = "Accessories", Price = 2500, RemainingStock = 8},
            new Product { Id = 202, Name = "smart glasses", Category = "Accessories", Price = 5000, RemainingStock = 6},

            new Product { Id = 301, Name = "tesla", Category = "Smart Cars", Price = 2500000, RemainingStock = 2},
            new Product { Id = 302, Name = "byd", Category = "Smart Cars", Price = 1800000, RemainingStock = 3}
        };
        int[] cartId = new int[10];
        int[] cartQty = new int[10];
        double[] cartTotal = new double[10];
        int cartCount = 0;
        string[] orderHistory = new string[100];
        int orderCount = 0;
        int receiptNo = 1;
        bool running = true;
 
        

        while (running)
        {
            Console.WriteLine("\n=== SHOPPING CART SYSTEM ===");
            Console.WriteLine("1. View Products\n2. Search Product\n3. Filter Category\n4. Add to Cart\n5. View Cart\n6. Remove Item\n7. Update Quantity\n8. Clear Cart\n9. Checkout\n10. Order History\n11. Exit");
            Console.Write("\nEnter choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\n=== PRODUCTS ===");
                    foreach (var p in products) p.DisplayProduct();
                    break;

                case 2: 
                    Console.Write("\nEnter product name: ");
                    string keyword = Console.ReadLine().ToLower();
                    bool foundSearch = false;
                    foreach (var p in products)
                    {
                        if (p.Name.ToLower().Contains(keyword))
                        {
                            p.DisplayProduct();
                            foundSearch = true;
                        }
                    }
                    if (!foundSearch) Console.WriteLine("No product found.");
                    break;

                case 3: 
                    Console.WriteLine("\n1. Electronics\n2. Accessories\n3. Smart Cars");
                    Console.Write("Select category: ");
                    if (int.TryParse(Console.ReadLine(), out int cat))
                    {
                        string selectedCat = cat == 1 ? "Electronics" : cat == 2 ? "Accessories" : cat == 3 ? "Smart Cars" : "";
                        if (selectedCat != "")
                        {
                            Console.WriteLine($"\n=== {selectedCat.ToUpper()} ===");
                            foreach (var p in products) if (p.Category == selectedCat) p.DisplayProduct();
                        }
                        else Console.WriteLine("Invalid category.");
                    }
                    break;

                case 4: 
                    Console.Write("\nEnter ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) break;
                    Product selected = Array.Find(products, p => p.Id == id);
                    if (selected == null) { Console.WriteLine("Not found."); break; }

                    Console.Write("Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) break;
                    if (!selected.HasEnoughStock(qty)) { Console.WriteLine("Not enough stock."); break; }

                    bool added = false;
                    for (int i = 0; i < cartCount; i++)
                    {
                        if (cartId[i] == selected.Id)
                        {
                            cartQty[i] += qty;
                            cartTotal[i] = selected.GetItemTotal(cartQty[i]);
                            added = true;
                        }
                    }
                    if (!added && cartCount < 10)
                    {
                        cartId[cartCount] = selected.Id;
                        cartQty[cartCount] = qty;
                        cartTotal[cartCount] = selected.GetItemTotal(qty);
                        cartCount++;
                    }
                    selected.DeductStock(qty);
                    Console.WriteLine("Added to cart!");
                    break;

                case 5: // View Cart
                    Console.WriteLine("\n=== CART ===");
                    double total = 0;
                    for (int i = 0; i < cartCount; i++)
                    {
                        var p = Array.Find(products, prod => prod.Id == cartId[i]);
                        Console.WriteLine($"{p.Name} x{cartQty[i]} = ₱{cartTotal[i]}");
                        total += cartTotal[i];
                    }
                    Console.WriteLine($"Total: ₱{total}");
                    break;

                case 6: 
                    Console.Write("Enter ID to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int rid))
                    {
                        for (int i = 0; i < cartCount; i++)
                        {
                            if (cartId[i] == rid)
                            {
                                var p = Array.Find(products, prod => prod.Id == rid);
                                p.AddStock(cartQty[i]); 
                                for (int j = i; j < cartCount - 1; j++)
                                {
                                    cartId[j] = cartId[j + 1];
                                    cartQty[j] = cartQty[j + 1];
                                    cartTotal[j] = cartTotal[j + 1];
                                }
                                cartCount--;
                                Console.WriteLine("Removed.");
                                break;
                            }
                        }
                    }
                    break;

                case 7: 
                    Console.Write("Enter ID: ");
                    if (int.TryParse(Console.ReadLine(), out int uid))
                    {
                        Console.Write("New qty: ");
                        if (int.TryParse(Console.ReadLine(), out int newQty))
                        {
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cartId[i] == uid)
                                {
                                    var p = Array.Find(products, prod => prod.Id == uid);
                                    p.AddStock(cartQty[i]); 
                                    if (p.HasEnoughStock(newQty))
                                    {
                                        p.DeductStock(newQty);
                                        cartQty[i] = newQty;
                                        cartTotal[i] = p.GetItemTotal(newQty);
                                        Console.WriteLine("Updated.");
                                    }
                                    else 
                                    { 
                                        p.DeductStock(cartQty[i]); 
                                        Console.WriteLine("Insufficient stock."); 
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 8: 
                    for (int i = 0; i < cartCount; i++)
                    {
                        var p = Array.Find(products, prod => prod.Id == cartId[i]);
                        p.AddStock(cartQty[i]);
                    }
                    cartCount = 0;
                    Console.WriteLine("Cart cleared.");
                    break;

                case 9: 
                    if (cartCount == 0) { Console.WriteLine("Cart is empty."); break; }
                    double grand = 0;
                    for (int i = 0; i < cartCount; i++) grand += cartTotal[i];
                    double disc = (grand >= 5000) ? grand * 0.10 : 0;
                    double fin = grand - disc;

                    Console.WriteLine("\n=== RECEIPT ===");
                    Console.WriteLine($"Receipt No: {receiptNo:D4} | Date: {DateTime.Now}");
                    Console.WriteLine($"Final Amount: ₱{fin}");

                    double pay = 0;
                    while (pay < fin)
                    {
                        Console.Write("Payment: ");
                        if (double.TryParse(Console.ReadLine(), out pay) && pay >= fin) break;
                        Console.WriteLine("Insufficient payment.");
                    }
                    Console.WriteLine($"Change: ₱{pay - fin}");
                    
                    orderHistory[orderCount++] = $"Receipt #{receiptNo:D4} - ₱{fin}";
                    receiptNo++;
                    Console.WriteLine("\nLOW STOCK ALERT:");
                    foreach (var p in products) 
                        if (p.RemainingStock <= 5) Console.WriteLine($"{p.Name}: {p.RemainingStock} left");
                    
                    cartCount = 0;
                    break;

                case 10: 
                    Console.WriteLine("\n=== ORDER HISTORY ===");
                    for (int i = 0; i < orderCount; i++) Console.WriteLine(orderHistory[i]);
                    break;

                case 11:
                    running = false;
                    break;
            }
        }
    }
}