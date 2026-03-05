Hệ Thống Quản Lý Thú Cưng (PetCare Management System)

Chào mừng bạn đến với dự án Quản Lý Thú Cưng. Đây là ứng dụng desktop được phát triển bằng ngôn ngữ C# trên nền tảng .NET, giúp quản lý thông tin thú cưng, lịch chăm sóc và các dịch vụ liên quan.
📌 Tính năng chính

    Quản lý danh sách thú cưng.

    Theo dõi tình trạng sức khỏe và lịch tiêm phòng.

    Quản lý thông tin chủ nuôi.

    Báo cáo thống kê dịch vụ ngay tại Dashboard của hệ thống.

🛠 Yêu cầu hệ thống

Để chạy được project này, máy tính của bạn cần cài đặt:

    Visual Studio (Phiên bản 2017 trở lên).

    .NET Framework (Phiên bản tương ứng với project, thường là 4.7.2 trở lên).

    SQL Server Management Studio (SSMS) để quản lý cơ sở dữ liệu.

    MySQL

🚀 Hướng dẫn cài đặt
1. Clone Project

Mở Terminal hoặc Git Bash và chạy lệnh sau:
2. Thiết lập Cơ sở dữ liệu ( Nếu chạy bị lỗi SQL ) Thường thì tải về build project là chạy được.

    Mở SQL Server Management Studio (SSMS).

    Tìm file script SQL trong thư mục dự án (thường nằm trong thư mục Database hoặc SQL).

    Chạy (Execute) file script để tạo Database và các bảng cần thiết.

3. Cấu hình Chuỗi kết nối (Connection String)

    Mở file App.config hoặc file chứa cấu hình kết nối trong Visual Studio.

    Cập nhật dòng connectionString sao cho khớp với tên Server SQL của bạn:

4. Chạy ứng dụng

    Mở file giải pháp .sln bằng Visual Studio.

    Nhấn Ctrl + Shift + B để Build project.

    Nhấn F5 hoặc nút Start để khởi chạy ứng dụng.

📂 Cấu trúc thư mục

    Các File Repository: Chứa các code Database của chức năng ấy và thao tác trực tiếp với cơ sở dữ liệu.

    [Name].xaml.cs: Xử lý nghiệp vụ.

    [Name].xaml: Chứa các code UI của từng chức năng

👥 Thành viên thực hiện

    KaiTrann (Trần Hùng Khánh) - Trưởng nhóm.
    Myth28-De (Nguyễn Đức Hướng) - Thành viên nhóm
    Luiss1901 (Trần Văn Lợi) - Thành viên nhóm
    nguyenthuanh25052005-hub (Nguyễn Thị Thu Ánh) - Thành viên nhóm
