using QuanLyThuCung.Core.Models;
using QuanLyThuCung.Core.Services.Implementations;

namespace QuanLyThuCung.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   HỆ THỐNG QUẢN LÝ CỬA HÀNG THÚ CƯNG  ║");
            Console.WriteLine("║    PET SHOP MANAGEMENT SYSTEM          ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();

            // Initialize services
            var petService = new PetService();
            var customerService = new CustomerService();
            var productService = new ProductService();
            var orderService = new OrderService();

            // Add sample data
            AddSampleData(petService, customerService, productService);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n═══════════════ MENU CHÍNH ═══════════════");
                Console.WriteLine("1. Quản lý thú cưng (Manage Pets)");
                Console.WriteLine("2. Quản lý khách hàng (Manage Customers)");
                Console.WriteLine("3. Quản lý sản phẩm (Manage Products)");
                Console.WriteLine("4. Quản lý đơn hàng (Manage Orders)");
                Console.WriteLine("5. Thoát (Exit)");
                Console.WriteLine("═════════════════════════════════════════");
                Console.Write("Chọn chức năng (Choose option): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManagePets(petService);
                        break;
                    case "2":
                        ManageCustomers(customerService);
                        break;
                    case "3":
                        ManageProducts(productService);
                        break;
                    case "4":
                        ManageOrders(orderService, customerService, productService);
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("\nCảm ơn bạn đã sử dụng hệ thống!");
                        Console.WriteLine("Thank you for using our system!");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ! (Invalid choice!)");
                        break;
                }
            }
        }

        static void AddSampleData(PetService petService, CustomerService customerService, ProductService productService)
        {
            // Add sample pets
            petService.AddPet(new Pet
            {
                Name = "Max",
                Species = "Chó (Dog)",
                Breed = "Golden Retriever",
                Age = 2,
                Price = 5000000,
                Color = "Vàng (Golden)",
                Gender = "Đực (Male)",
                IsAvailable = true,
                Description = "Chó Golden Retriever thân thiện và dễ thương"
            });

            petService.AddPet(new Pet
            {
                Name = "Luna",
                Species = "Mèo (Cat)",
                Breed = "Persian",
                Age = 1,
                Price = 3000000,
                Color = "Trắng (White)",
                Gender = "Cái (Female)",
                IsAvailable = true,
                Description = "Mèo Ba Tư lông trắng mượt mà"
            });

            petService.AddPet(new Pet
            {
                Name = "Rocky",
                Species = "Chó (Dog)",
                Breed = "Husky",
                Age = 3,
                Price = 7000000,
                Color = "Xám trắng (Gray & White)",
                Gender = "Đực (Male)",
                IsAvailable = true,
                Description = "Husky năng động và thông minh"
            });

            // Add sample customers
            customerService.AddCustomer(new Customer
            {
                Name = "Nguyễn Văn A",
                Email = "nguyenvana@email.com",
                Phone = "0901234567",
                Address = "123 Đường ABC, Quận 1, TP.HCM"
            });

            customerService.AddCustomer(new Customer
            {
                Name = "Trần Thị B",
                Email = "tranthib@email.com",
                Phone = "0912345678",
                Address = "456 Đường XYZ, Quận 2, TP.HCM"
            });

            // Add sample products
            productService.AddProduct(new Product
            {
                Name = "Thức ăn cho chó (Dog Food)",
                Category = "Thức ăn (Food)",
                Description = "Thức ăn chất lượng cao cho chó trưởng thành",
                Price = 200000,
                StockQuantity = 50,
                Supplier = "Pet Nutrition Co.",
                IsActive = true
            });

            productService.AddProduct(new Product
            {
                Name = "Thức ăn cho mèo (Cat Food)",
                Category = "Thức ăn (Food)",
                Description = "Thức ăn dinh dưỡng cho mèo",
                Price = 150000,
                StockQuantity = 40,
                Supplier = "Pet Nutrition Co.",
                IsActive = true
            });

            productService.AddProduct(new Product
            {
                Name = "Đồ chơi bóng (Ball Toy)",
                Category = "Đồ chơi (Toys)",
                Description = "Đồ chơi bóng cho thú cưng",
                Price = 50000,
                StockQuantity = 100,
                Supplier = "Pet Toys Ltd.",
                IsActive = true
            });

            Console.WriteLine("\n✓ Đã thêm dữ liệu mẫu (Sample data added successfully)");
        }

        static void ManagePets(PetService petService)
        {
            Console.WriteLine("\n═══ QUẢN LÝ THÚ CƯNG (PET MANAGEMENT) ═══");
            Console.WriteLine("1. Xem tất cả thú cưng (View all pets)");
            Console.WriteLine("2. Tìm kiếm thú cưng (Search pets)");
            Console.WriteLine("3. Thêm thú cưng mới (Add new pet)");
            Console.Write("Chọn: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var pets = petService.GetAllPets();
                    Console.WriteLine($"\n📋 Danh sách thú cưng ({pets.Count} con):");
                    foreach (var pet in pets)
                    {
                        Console.WriteLine($"\n  ID: {pet.Id}");
                        Console.WriteLine($"  Tên: {pet.Name}");
                        Console.WriteLine($"  Loài: {pet.Species} - Giống: {pet.Breed}");
                        Console.WriteLine($"  Tuổi: {pet.Age} | Giới tính: {pet.Gender}");
                        Console.WriteLine($"  Giá: {pet.Price:N0} VNĐ");
                        Console.WriteLine($"  Trạng thái: {(pet.IsAvailable ? "Có sẵn ✓" : "Đã bán")}");
                    }
                    break;

                case "2":
                    Console.Write("Nhập từ khóa tìm kiếm: ");
                    string? keyword = Console.ReadLine();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var searchResults = petService.SearchPets(keyword);
                        Console.WriteLine($"\n🔍 Tìm thấy {searchResults.Count} kết quả:");
                        foreach (var pet in searchResults)
                        {
                            Console.WriteLine($"  - {pet.Name} ({pet.Species} - {pet.Breed})");
                        }
                    }
                    break;

                case "3":
                    Console.WriteLine("\n➕ Thêm thú cưng mới:");
                    var newPet = new Pet { IsAvailable = true };
                    
                    Console.Write("Tên: ");
                    newPet.Name = Console.ReadLine() ?? "";
                    
                    Console.Write("Loài: ");
                    newPet.Species = Console.ReadLine() ?? "";
                    
                    Console.Write("Giống: ");
                    newPet.Breed = Console.ReadLine() ?? "";
                    
                    Console.Write("Tuổi: ");
                    if (int.TryParse(Console.ReadLine(), out int age))
                        newPet.Age = age;
                    
                    Console.Write("Giá: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                        newPet.Price = price;
                    
                    Console.Write("Màu sắc: ");
                    newPet.Color = Console.ReadLine() ?? "";
                    
                    Console.Write("Giới tính: ");
                    newPet.Gender = Console.ReadLine() ?? "";
                    
                    Console.Write("Mô tả: ");
                    newPet.Description = Console.ReadLine() ?? "";
                    
                    petService.AddPet(newPet);
                    Console.WriteLine($"✓ Đã thêm thú cưng #{newPet.Id} thành công!");
                    break;
            }
        }

        static void ManageCustomers(CustomerService customerService)
        {
            Console.WriteLine("\n═══ QUẢN LÝ KHÁCH HÀNG (CUSTOMER MANAGEMENT) ═══");
            Console.WriteLine("1. Xem tất cả khách hàng (View all customers)");
            Console.WriteLine("2. Tìm kiếm khách hàng (Search customers)");
            Console.WriteLine("3. Thêm khách hàng mới (Add new customer)");
            Console.Write("Chọn: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var customers = customerService.GetAllCustomers();
                    Console.WriteLine($"\n📋 Danh sách khách hàng ({customers.Count} người):");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"\n  ID: {customer.Id}");
                        Console.WriteLine($"  Tên: {customer.Name}");
                        Console.WriteLine($"  Email: {customer.Email}");
                        Console.WriteLine($"  SĐT: {customer.Phone}");
                        Console.WriteLine($"  Địa chỉ: {customer.Address}");
                        Console.WriteLine($"  Ngày đăng ký: {customer.DateRegistered:dd/MM/yyyy}");
                    }
                    break;

                case "2":
                    Console.Write("Nhập từ khóa tìm kiếm: ");
                    string? keyword = Console.ReadLine();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var searchResults = customerService.SearchCustomers(keyword);
                        Console.WriteLine($"\n🔍 Tìm thấy {searchResults.Count} kết quả:");
                        foreach (var customer in searchResults)
                        {
                            Console.WriteLine($"  - {customer.Name} | {customer.Phone} | {customer.Email}");
                        }
                    }
                    break;

                case "3":
                    Console.WriteLine("\n➕ Thêm khách hàng mới:");
                    var newCustomer = new Customer();
                    
                    Console.Write("Tên: ");
                    newCustomer.Name = Console.ReadLine() ?? "";
                    
                    Console.Write("Email: ");
                    newCustomer.Email = Console.ReadLine() ?? "";
                    
                    Console.Write("Số điện thoại: ");
                    newCustomer.Phone = Console.ReadLine() ?? "";
                    
                    Console.Write("Địa chỉ: ");
                    newCustomer.Address = Console.ReadLine() ?? "";
                    
                    customerService.AddCustomer(newCustomer);
                    Console.WriteLine($"✓ Đã thêm khách hàng #{newCustomer.Id} thành công!");
                    break;
            }
        }

        static void ManageProducts(ProductService productService)
        {
            Console.WriteLine("\n═══ QUẢN LÝ SẢN PHẨM (PRODUCT MANAGEMENT) ═══");
            Console.WriteLine("1. Xem tất cả sản phẩm (View all products)");
            Console.WriteLine("2. Tìm kiếm sản phẩm (Search products)");
            Console.WriteLine("3. Thêm sản phẩm mới (Add new product)");
            Console.Write("Chọn: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var products = productService.GetAllProducts();
                    Console.WriteLine($"\n📋 Danh sách sản phẩm ({products.Count} sản phẩm):");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"\n  ID: {product.Id}");
                        Console.WriteLine($"  Tên: {product.Name}");
                        Console.WriteLine($"  Loại: {product.Category}");
                        Console.WriteLine($"  Giá: {product.Price:N0} VNĐ");
                        Console.WriteLine($"  Tồn kho: {product.StockQuantity} đơn vị");
                        Console.WriteLine($"  Nhà cung cấp: {product.Supplier}");
                    }
                    break;

                case "2":
                    Console.Write("Nhập từ khóa tìm kiếm: ");
                    string? keyword = Console.ReadLine();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var searchResults = productService.SearchProducts(keyword);
                        Console.WriteLine($"\n🔍 Tìm thấy {searchResults.Count} kết quả:");
                        foreach (var product in searchResults)
                        {
                            Console.WriteLine($"  - {product.Name} | {product.Category} | {product.Price:N0} VNĐ");
                        }
                    }
                    break;

                case "3":
                    Console.WriteLine("\n➕ Thêm sản phẩm mới:");
                    var newProduct = new Product { IsActive = true };
                    
                    Console.Write("Tên sản phẩm: ");
                    newProduct.Name = Console.ReadLine() ?? "";
                    
                    Console.Write("Loại (Food/Toys/Accessories): ");
                    newProduct.Category = Console.ReadLine() ?? "";
                    
                    Console.Write("Mô tả: ");
                    newProduct.Description = Console.ReadLine() ?? "";
                    
                    Console.Write("Giá: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                        newProduct.Price = price;
                    
                    Console.Write("Số lượng tồn kho: ");
                    if (int.TryParse(Console.ReadLine(), out int stock))
                        newProduct.StockQuantity = stock;
                    
                    Console.Write("Nhà cung cấp: ");
                    newProduct.Supplier = Console.ReadLine() ?? "";
                    
                    productService.AddProduct(newProduct);
                    Console.WriteLine($"✓ Đã thêm sản phẩm #{newProduct.Id} thành công!");
                    break;
            }
        }

        static void ManageOrders(OrderService orderService, CustomerService customerService, ProductService productService)
        {
            Console.WriteLine("\n═══ QUẢN LÝ ĐỐN HÀNG (ORDER MANAGEMENT) ═══");
            Console.WriteLine("1. Xem tất cả đơn hàng (View all orders)");
            Console.WriteLine("2. Tạo đơn hàng mới (Create new order)");
            Console.Write("Chọn: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var orders = orderService.GetAllOrders();
                    Console.WriteLine($"\n📋 Danh sách đơn hàng ({orders.Count} đơn):");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"\n  Đơn hàng #{order.Id}");
                        Console.WriteLine($"  Khách hàng ID: {order.CustomerId}");
                        Console.WriteLine($"  Ngày đặt: {order.OrderDate:dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"  Tổng tiền: {order.TotalAmount:N0} VNĐ");
                        Console.WriteLine($"  Trạng thái: {order.Status}");
                        Console.WriteLine($"  Phương thức: {order.PaymentMethod}");
                    }
                    break;

                case "2":
                    Console.WriteLine("\n➕ Tạo đơn hàng mới:");
                    
                    Console.Write("Nhập ID khách hàng: ");
                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("ID không hợp lệ!");
                        break;
                    }

                    var customer = customerService.GetCustomerById(customerId);
                    if (customer == null)
                    {
                        Console.WriteLine("Không tìm thấy khách hàng!");
                        break;
                    }

                    var newOrder = new Order
                    {
                        CustomerId = customerId,
                        Customer = customer,
                        Status = "Pending",
                        OrderItems = new List<OrderItem>()
                    };

                    Console.Write("Phương thức thanh toán (Cash/Card/Transfer): ");
                    newOrder.PaymentMethod = Console.ReadLine() ?? "Cash";

                    bool addingItems = true;
                    while (addingItems)
                    {
                        Console.Write("\nNhập ID sản phẩm (hoặc 0 để kết thúc): ");
                        if (!int.TryParse(Console.ReadLine(), out int productId) || productId == 0)
                        {
                            addingItems = false;
                            continue;
                        }

                        var product = productService.GetProductById(productId);
                        if (product == null)
                        {
                            Console.WriteLine("Không tìm thấy sản phẩm!");
                            continue;
                        }

                        Console.Write("Số lượng: ");
                        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                        {
                            Console.WriteLine("Số lượng không hợp lệ!");
                            continue;
                        }

                        var orderItem = new OrderItem
                        {
                            ProductId = productId,
                            Product = product,
                            Quantity = quantity,
                            UnitPrice = product.Price,
                            Subtotal = product.Price * quantity
                        };

                        newOrder.OrderItems.Add(orderItem);
                        Console.WriteLine($"✓ Đã thêm: {product.Name} x{quantity} = {orderItem.Subtotal:N0} VNĐ");
                    }

                    if (newOrder.OrderItems.Count > 0)
                    {
                        orderService.CreateOrder(newOrder);
                        Console.WriteLine($"\n✓ Đã tạo đơn hàng #{newOrder.Id} thành công!");
                        Console.WriteLine($"  Tổng tiền: {newOrder.TotalAmount:N0} VNĐ");
                    }
                    else
                    {
                        Console.WriteLine("Đơn hàng không có sản phẩm nào!");
                    }
                    break;
            }
        }
    }
}
