# QuanLyThuCung - Pet Shop Management System

A comprehensive C# application for managing a pet shop, developed using Visual Studio.

## 🐾 Features

### Core Modules
1. **Pet Management (Quản lý thú cưng)**
   - Add, view, update, and delete pet information
   - Track pet details: name, species, breed, age, price, color, gender
   - Search pets by name, species, or breed
   - Monitor availability status

2. **Customer Management (Quản lý khách hàng)**
   - Manage customer information
   - Track customer contact details and addresses
   - Search customers by name, email, or phone
   - View customer purchase history

3. **Product Management (Quản lý sản phẩm)**
   - Manage pet supplies, food, toys, and accessories
   - Track stock levels and pricing
   - Categorize products
   - Search products by name or category

4. **Order Management (Quản lý đơn hàng)**
   - Create and manage customer orders
   - Track order status and payment methods
   - Calculate order totals automatically
   - View order history

## 🏗️ Project Structure

```
QuanLyThuCung/
├── QuanLyThuCung.sln                    # Visual Studio Solution
├── QuanLyThuCung.Core/                  # Core Business Logic Library
│   ├── Models/                          # Data Models
│   │   ├── Pet.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Product.cs
│   │   └── Employee.cs
│   └── Services/                        # Business Services
│       ├── Interfaces/                  # Service Interfaces
│       │   ├── IPetService.cs
│       │   ├── ICustomerService.cs
│       │   ├── IOrderService.cs
│       │   └── IProductService.cs
│       └── Implementations/             # Service Implementations
│           ├── PetService.cs
│           ├── CustomerService.cs
│           ├── OrderService.cs
│           └── ProductService.cs
└── QuanLyThuCung.App/                   # Console Application
    └── Program.cs                       # Main Entry Point
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022 or later
- .NET 8.0 SDK or later

### Opening the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/KaiTrann/QuanLyThuCung.git
   cd QuanLyThuCung
   ```

2. Open the solution in Visual Studio:
   - Double-click `QuanLyThuCung.sln`, or
   - Open Visual Studio → File → Open → Project/Solution → Select `QuanLyThuCung.sln`

3. Build the solution:
   - Press `Ctrl+Shift+B`, or
   - Menu: Build → Build Solution

4. Run the application:
   - Press `F5` (Debug mode), or
   - Press `Ctrl+F5` (Run without debugging)

### Using Command Line
```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project QuanLyThuCung.App/QuanLyThuCung.App.csproj
```

## 📝 Usage

The application provides a console-based menu interface with the following options:

1. **Quản lý thú cưng (Manage Pets)**
   - View all pets in the shop
   - Search for specific pets
   - Add new pets to the inventory

2. **Quản lý khách hàng (Manage Customers)**
   - View all registered customers
   - Search for specific customers
   - Register new customers

3. **Quản lý sản phẩm (Manage Products)**
   - View all products and supplies
   - Search for specific products
   - Add new products to inventory

4. **Quản lý đơn hàng (Manage Orders)**
   - View all orders
   - Create new orders for customers

## 👥 Team Members

This project is developed by a team of 4 members. Each member can work on different modules:
- Member 1: Pet Management Module
- Member 2: Customer Management Module
- Member 3: Product Management Module
- Member 4: Order Management Module

## 🔧 Development

### Adding New Features
The project follows a modular architecture:

1. **Add a new model**: Create a class in `QuanLyThuCung.Core/Models/`
2. **Add a service interface**: Create an interface in `QuanLyThuCung.Core/Services/Interfaces/`
3. **Implement the service**: Create implementation in `QuanLyThuCung.Core/Services/Implementations/`
4. **Use in the app**: Update `Program.cs` to use the new service

### Converting to Windows Forms
To convert this console application to Windows Forms:

1. Add a new Windows Forms App project to the solution:
   ```
   Right-click solution → Add → New Project → Windows Forms App
   ```

2. Add reference to `QuanLyThuCung.Core`:
   ```
   Right-click on the new project → Add → Project Reference → Select QuanLyThuCung.Core
   ```

3. Design forms for each module (Pet, Customer, Product, Order management)

4. Use the existing services from `QuanLyThuCung.Core` in your forms

## 📦 Future Enhancements

- [ ] Add database integration (SQL Server/SQLite)
- [ ] Implement user authentication
- [ ] Add reporting features
- [ ] Create Windows Forms UI
- [ ] Add export to Excel functionality
- [ ] Implement backup and restore
- [ ] Add image support for pets
- [ ] Create a REST API

## 📄 License

This project is open source and available for educational purposes.

## 🤝 Contributing

Team members can contribute by:
1. Creating a new branch for their feature
2. Making changes in their branch
3. Creating a pull request for review
4. Merging after approval

## 📞 Contact

For questions or support, please contact the development team.

---

**Note**: This is a console application that demonstrates the core business logic. The project is structured to be easily extended with a Windows Forms, WPF, or ASP.NET Core frontend.

