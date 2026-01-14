using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using StudentManagementSystem.Utils;

class Program
{
    private static StudentServices _studentServices = null!;
    
    public static string GetProjectRoot()
    {
        return Directory
            .GetParent(AppContext.BaseDirectory)!
            .Parent!
            .Parent!
            .Parent!
            .FullName;
    }

    static async Task Main(string[] args)
    {

        var projectRoot = GetProjectRoot();
        Console.WriteLine(projectRoot);
        var jsonPath = Path.Combine(
            projectRoot,
            "Data",
            "students.json"
        );

        //if (!File.Exists(jsonPath))
        //{
        //    var generator = new DataGenerator(
        //        new List<Student>(),
        //        new JsonServices(jsonPath)
        //    );

        //    await generator.GenerateStudent(1000);
        //}

        var jsonServices = new JsonServices(jsonPath);
        _studentServices = new StudentServices(jsonServices);

        await RunAsync();
    }

    private static async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== HỆ THỐNG QUẢN LÝ SINH VIÊN =====");
            Console.WriteLine("1. Thêm sinh viên");
            Console.WriteLine("2. Sửa sinh viên theo Id");
            Console.WriteLine("3. Xóa sinh viên theo Id");
            Console.WriteLine("4. Hiển thị danh sách sinh viên");
            Console.WriteLine("5. Tìm sinh viên theo Id");
            Console.WriteLine("6. Danh sách sinh viên GPA >= 8");
            Console.WriteLine("7. Top 5 sinh viên GPA cao nhất");
            Console.WriteLine("0. Thoát");
            Console.Write("Lựa chọn: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                continue;
            }

            switch (choice)
            {
                case 1:
                    await AddMenuAsync();
                    break;

                case 2:
                    await _studentServices.UpdateByIdAsync();
                    break;

                case 3:
                    await _studentServices.DeleteByIdAsync();
                    break;

                case 4:
                    await _studentServices.ShowAllAsync();
                    break;

                case 5:
                    await _studentServices.FindByIdAsync();
                    break;

                case 6:
                    await _studentServices.ListGpaAbove8Async();
                    break;

                case 7:
                    await _studentServices.TopGpaAsync();
                    break;

                case 0:
                    Console.WriteLine("Tạm biệt");
                    return;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ");
                    break;
            }
        }
    }

    private static async Task AddMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("===== CÁCH THÊM =====");
            Console.WriteLine("1. Thêm thủ công");
            Console.WriteLine("2. Thêm từ file JSON");
            Console.WriteLine("0. Quay trở lại");
            Console.Write("Lựa chọn: ");

            if (!int.TryParse(Console.ReadLine(), out int c))
                continue;

            switch (c)
            {
                case 1:
                    await _studentServices.AddManualAsync();
                    break;

                case 2:
                    await _studentServices.ImportFromJsonAsync();
                    break;

                case 0:
                    back = true;
                    break;

                default:
                    Console.WriteLine("Nhập sai lựa chọn. Vui lòng chọn lại");
                    break;
            }
        }
    }
}
