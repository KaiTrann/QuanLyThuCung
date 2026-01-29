# Hướng dẫn Phát triển - Development Guide

## Cấu trúc Dự án / Project Structure

### 1. QuanLyThuCung.Core (Class Library)
Thư viện chứa logic nghiệp vụ và mô hình dữ liệu.
**Core business logic and data models library.**

#### Models (Mô hình dữ liệu)
- `Pet.cs`: Thông tin thú cưng
- `Customer.cs`: Thông tin khách hàng
- `Product.cs`: Thông tin sản phẩm (thức ăn, đồ chơi, phụ kiện)
- `Order.cs`: Đơn hàng
- `OrderItem.cs`: Chi tiết sản phẩm trong đơn hàng
- `Employee.cs`: Thông tin nhân viên

#### Services (Dịch vụ)
**Interfaces**: Định nghĩa các phương thức
- `IPetService.cs`: Quản lý thú cưng
- `ICustomerService.cs`: Quản lý khách hàng
- `IProductService.cs`: Quản lý sản phẩm
- `IOrderService.cs`: Quản lý đơn hàng

**Implementations**: Triển khai các phương thức
- `PetService.cs`
- `CustomerService.cs`
- `ProductService.cs`
- `OrderService.cs`

### 2. QuanLyThuCung.App (Console Application)
Ứng dụng console với giao diện menu tiếng Việt.
**Console application with Vietnamese menu interface.**

## Phân công Công việc / Task Assignment

Mỗi thành viên có thể phụ trách một module:

### Member 1: Quản lý Thú cưng / Pet Management
- File: `QuanLyThuCung.Core/Models/Pet.cs`
- File: `QuanLyThuCung.Core/Services/Interfaces/IPetService.cs`
- File: `QuanLyThuCung.Core/Services/Implementations/PetService.cs`
- Tính năng: CRUD thú cưng, tìm kiếm, lọc theo loài/giống

### Member 2: Quản lý Khách hàng / Customer Management
- File: `QuanLyThuCung.Core/Models/Customer.cs`
- File: `QuanLyThuCung.Core/Services/Interfaces/ICustomerService.cs`
- File: `QuanLyThuCung.Core/Services/Implementations/CustomerService.cs`
- Tính năng: CRUD khách hàng, lịch sử mua hàng

### Member 3: Quản lý Sản phẩm / Product Management
- File: `QuanLyThuCung.Core/Models/Product.cs`
- File: `QuanLyThuCung.Core/Services/Interfaces/IProductService.cs`
- File: `QuanLyThuCung.Core/Services/Implementations/ProductService.cs`
- Tính năng: CRUD sản phẩm, quản lý tồn kho, phân loại

### Member 4: Quản lý Đơn hàng / Order Management
- File: `QuanLyThuCung.Core/Models/Order.cs`
- File: `QuanLyThuCung.Core/Models/OrderItem.cs`
- File: `QuanLyThuCung.Core/Services/Interfaces/IOrderService.cs`
- File: `QuanLyThuCung.Core/Services/Implementations/OrderService.cs`
- Tính năng: Tạo đơn, cập nhật trạng thái, báo cáo doanh thu

## Hướng dẫn Git / Git Workflow

### 1. Clone Repository
```bash
git clone https://github.com/KaiTrann/QuanLyThuCung.git
cd QuanLyThuCung
```

### 2. Tạo Branch mới / Create New Branch
```bash
# Member 1 - Pet Module
git checkout -b feature/pet-management

# Member 2 - Customer Module
git checkout -b feature/customer-management

# Member 3 - Product Module
git checkout -b feature/product-management

# Member 4 - Order Module
git checkout -b feature/order-management
```

### 3. Làm việc trên Branch / Work on Your Branch
```bash
# Xem trạng thái
git status

# Thêm file vào staging
git add .

# Commit changes
git commit -m "Add: Feature description"

# Push lên GitHub
git push origin feature/your-branch-name
```

### 4. Tạo Pull Request
1. Vào GitHub repository
2. Click "Pull requests"
3. Click "New pull request"
4. Chọn branch của bạn → Create pull request
5. Đợi review và merge

### 5. Cập nhật từ Main Branch
```bash
git checkout main
git pull origin main
git checkout feature/your-branch-name
git merge main
```

## Quy ước Đặt tên / Naming Conventions

### Classes và Interfaces
- PascalCase: `PetService`, `ICustomerService`
- Interface bắt đầu với "I": `IPetService`

### Methods
- PascalCase: `GetAllPets()`, `AddCustomer()`
- Động từ + Danh từ: `GetPetById()`, `UpdateProduct()`

### Properties
- PascalCase: `Name`, `Price`, `IsAvailable`

### Private Fields
- camelCase với "_": `_pets`, `_nextId`

### Variables
- camelCase: `petId`, `customerName`

## Cách Thêm Tính năng Mới / Adding New Features

### 1. Thêm Model mới
```csharp
// File: QuanLyThuCung.Core/Models/YourModel.cs
namespace QuanLyThuCung.Core.Models
{
    public class YourModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // ... other properties
    }
}
```

### 2. Tạo Service Interface
```csharp
// File: QuanLyThuCung.Core/Services/Interfaces/IYourService.cs
namespace QuanLyThuCung.Core.Services.Interfaces
{
    public interface IYourService
    {
        List<YourModel> GetAll();
        YourModel? GetById(int id);
        void Add(YourModel model);
        void Update(YourModel model);
        void Delete(int id);
    }
}
```

### 3. Triển khai Service
```csharp
// File: QuanLyThuCung.Core/Services/Implementations/YourService.cs
namespace QuanLyThuCung.Core.Services.Implementations
{
    public class YourService : IYourService
    {
        private readonly List<YourModel> _items = new();
        private int _nextId = 1;
        
        public List<YourModel> GetAll() => _items.ToList();
        
        public YourModel? GetById(int id) 
            => _items.FirstOrDefault(x => x.Id == id);
        
        public void Add(YourModel model)
        {
            model.Id = _nextId++;
            _items.Add(model);
        }
        
        // Implement other methods...
    }
}
```

### 4. Sử dụng trong App
```csharp
// File: QuanLyThuCung.App/Program.cs
var yourService = new YourService();
```

## Build và Test / Building and Testing

### Build Project
```bash
# Build tất cả
dotnet build

# Build Core library
dotnet build QuanLyThuCung.Core/QuanLyThuCung.Core.csproj

# Build App
dotnet build QuanLyThuCung.App/QuanLyThuCung.App.csproj
```

### Run Application
```bash
# Chạy ứng dụng
dotnet run --project QuanLyThuCung.App/QuanLyThuCung.App.csproj

# Hoặc từ Visual Studio: F5 (Debug) hoặc Ctrl+F5 (Run)
```

### Clean Build
```bash
dotnet clean
dotnet build
```

## Chuyển sang Windows Forms / Converting to Windows Forms

### 1. Tạo Project mới trong Visual Studio
1. Right-click Solution → Add → New Project
2. Chọn "Windows Forms App (.NET)"
3. Tên: `QuanLyThuCung.WinForms`
4. Framework: .NET 8.0

### 2. Thêm Reference
1. Right-click `QuanLyThuCung.WinForms` → Add → Project Reference
2. Check: `QuanLyThuCung.Core`
3. Click OK

### 3. Tạo Forms
- `FormMain.cs`: Form chính với menu
- `FormPets.cs`: Quản lý thú cưng
- `FormCustomers.cs`: Quản lý khách hàng
- `FormProducts.cs`: Quản lý sản phẩm
- `FormOrders.cs`: Quản lý đơn hàng

### 4. Sử dụng Services
```csharp
public partial class FormPets : Form
{
    private readonly IPetService _petService;
    
    public FormPets()
    {
        InitializeComponent();
        _petService = new PetService();
        LoadPets();
    }
    
    private void LoadPets()
    {
        var pets = _petService.GetAllPets();
        dataGridView1.DataSource = pets;
    }
}
```

## Tips và Best Practices

### 1. Code Style
- Sử dụng tiếng Anh cho code, comment
- Sử dụng tiếng Việt cho UI messages
- Thêm XML comments cho public methods

### 2. Git Best Practices
- Commit thường xuyên với message rõ ràng
- Pull trước khi push
- Không commit file bin/, obj/, .vs/
- Review code trước khi merge

### 3. Testing
- Test từng tính năng sau khi code
- Test các trường hợp edge case
- Test integration giữa các module

### 4. Documentation
- Cập nhật README khi thêm tính năng mới
- Thêm comments cho logic phức tạp
- Document các API methods

## Troubleshooting

### Build Error
```bash
# Clean và rebuild
dotnet clean
dotnet restore
dotnet build
```

### Git Conflicts
```bash
# Xem conflicts
git status

# Edit file để resolve conflicts
# Sau đó:
git add .
git commit -m "Resolve merge conflicts"
```

### Visual Studio Issues
- Delete bin/ và obj/ folders
- Rebuild solution
- Restart Visual Studio

## Resources

### .NET Documentation
- https://docs.microsoft.com/en-us/dotnet/

### C# Programming Guide
- https://docs.microsoft.com/en-us/dotnet/csharp/

### Windows Forms
- https://docs.microsoft.com/en-us/dotnet/desktop/winforms/

### Git Guide
- https://git-scm.com/doc

## Contact Team Members

Liên hệ với các thành viên team khi:
- Gặp bug không tự fix được
- Cần review code
- Có conflict trong merge
- Cần help với tính năng mới

---

**Happy Coding! 🐾**
