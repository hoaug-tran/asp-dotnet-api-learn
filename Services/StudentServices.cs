using System.Text.Json;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class StudentServices
    {
        private readonly JsonServices _jsonServices;

        public StudentServices(JsonServices jsonServices)
        {
            _jsonServices = jsonServices;
        }

        public static string GetProjectRoot()
        {
            return Directory
                .GetParent(AppContext.BaseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .FullName;
        }

        public static void Table()
        {
            Console.WriteLine($"{"ID",-5} {"Họ và tên",-25} {"GPA",-10} {"Avatar"}");
        }

        // thêm thủ công
        public async Task AddManualAsync()
        {
            var students = await _jsonServices.LoadAsync();
            int nextId = students.Any() ? students.Max(s => s.Id) + 1 : 1;

            string fullName;
            do
            {
                Console.Write("Nhập họ tên: ");
                fullName = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrWhiteSpace(fullName));

            double gpa;
            while (true)
            {
                Console.Write("Nhập GPA (0–10): ");
                if (double.TryParse(Console.ReadLine(), out gpa) && gpa >= 0 && gpa <= 10)
                {
                    break;
                }
                Console.WriteLine("GPA không hợp lệ.");
            }

            string avatarPath = "";

            Console.Write("Bạn có muốn thêm avatar (Y/N)? ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
            {
                Console.Write("Nhập đường dẫn ảnh: ");
                var inputPath = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(inputPath) && File.Exists(inputPath))
                {
                    var projectRoot = GetProjectRoot();
                    var imageDir = Path.Combine(projectRoot, "Data", "images");
                    Directory.CreateDirectory(imageDir);

                    var ext = Path.GetExtension(inputPath);
                    var fileName = $"student_{nextId}{ext}";
                    var destPath = Path.Combine(imageDir, fileName);

                    File.Copy(inputPath, destPath, overwrite: true);
                    avatarPath = Path.Combine("Data", "images", fileName); 
                }
            }

            students.Add(new Student(nextId, fullName, gpa, avatarPath));
            await _jsonServices.SaveAsync(students);

            Console.WriteLine("Thêm sinh viên thành công");
        }

        // thêm từ json
        public async Task ImportFromJsonAsync()
        {
            Console.Write("Nhập đường dẫn file JSON: ");
            var path = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("File không tồn tại.");
                return;
            }

            using FileStream fs = File.OpenRead(path);
            var imported = await JsonSerializer.DeserializeAsync<List<Student>>(fs) ?? new List<Student>();

            if (imported.Count == 0)
            {
                Console.WriteLine("File không có dữ liệu.");
                return;
            }

            var students = await _jsonServices.LoadAsync();
            int nextId = students.Any() ? students.Max(s => s.Id) + 1 : 1;

            int addedCount = 0;

            foreach (var s in imported)
            {
                if (students.Any(x => x.FullName == s.FullName && x.GPA == s.GPA))
                    continue;

                s.Id = nextId++;
                students.Add(s);
                addedCount++;
            }

            await _jsonServices.SaveAsync(students);
            Console.WriteLine($"Đã nhập {addedCount} sinh viên từ file JSON");
        }

        // sửa
        public async Task UpdateByIdAsync()
        {
            var students = await _jsonServices.LoadAsync();

            Console.Write("Nhập Id cần sửa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                return;
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                Console.WriteLine($"Không tìm thấy sinh viên có ID là {id}");
                return;
            }

            bool exit = true;
            while (exit)
            {
                Console.WriteLine("===== LỰA CHỌN SỬA =====");
                Console.WriteLine("1. Sửa tên");
                Console.WriteLine("2. Sửa GPA");
                Console.WriteLine("3. Sửa avatar");
                Console.WriteLine("4. Sửa toàn bộ thông tin");
                Console.WriteLine("0. Lưu và thoát");
                Console.Write("Lựa chọn: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    continue;
                }

                switch (choice)
                {
                    case 1:
                    {
                        Console.Write($"Nhập tên mới ({student.FullName}): ");
                        var name = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            student.FullName = name;
                        }

                        break;
                    }

                    case 2:
                    {
                        Console.Write($"Nhập GPA mới ({student.GPA}): ");
                        if (double.TryParse(Console.ReadLine(), out double gpa) && gpa >= 0 && gpa <= 10)
                        {
                            student.GPA = gpa;
                        }

                        break;
                    }

                    case 3:
                    {
                        Console.Write($"Nhập đường dẫn avatar mới: ");
                        var inputPath = Console.ReadLine()?.Trim();

                        if (!string.IsNullOrWhiteSpace(inputPath) && File.Exists(inputPath))
                        {
                            var projectRoot = GetProjectRoot();
                            var imageDir = Path.Combine(projectRoot, "Data", "images");
                            Directory.CreateDirectory(imageDir);

                                var ext = Path.GetExtension(inputPath);
                            var fileName = $"student_{student.Id}{ext}";
                            var destPath = Path.Combine(imageDir, fileName);

                            File.Copy(inputPath, destPath, overwrite: true);
                            student.ImagePath = Path.Combine("Data", "images", fileName); 
                        }

                        break;
                    }


                    case 4:
                    {
                        Console.Write($"Nhập tên mới ({student.FullName}): ");
                        var name = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            student.FullName = name;
                        }

                        Console.Write($"Nhập GPA mới ({student.GPA}): ");
                        if (double.TryParse(Console.ReadLine(), out double gpa) && gpa >= 0 && gpa <= 10)
                        {
                            student.GPA = gpa;
                        }

                        Console.Write($"Nhập đường dẫn avatar mới: ");
                        var inputPath = Console.ReadLine()?.Trim();

                        if (!string.IsNullOrWhiteSpace(inputPath) && File.Exists(inputPath))
                        {
                            var projectRoot = GetProjectRoot();
                            var imageDir = Path.Combine(projectRoot, "Data", "images");
                            Directory.CreateDirectory(imageDir);

                            var ext = Path.GetExtension(inputPath);
                            var fileName = $"student_{student.Id}{ext}";
                            var destPath = Path.Combine(imageDir, fileName);

                            File.Copy(inputPath, destPath, overwrite: true);
                            student.ImagePath = Path.Combine("Data", "images", fileName);
                        }

                        break;
                    }

                    case 0:
                    {
                        exit = false;
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập lại !");
                        break;
                    }
                }
            }

            await _jsonServices.SaveAsync(students);
            Console.WriteLine($"Cập nhật thành công sinh viên với ID: {student.Id}");
        }

        // xoá
        public async Task DeleteByIdAsync()
        {
            var students = await _jsonServices.LoadAsync();

            Console.Write("Nhập Id cần xóa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                return;
            }

            var removed = students.RemoveAll(s => s.Id == id);
            if (removed == 0)
            {
                Console.WriteLine($"Không tìm thấy sinh viên có ID là {id}");
                return;
            }

            await _jsonServices.SaveAsync(students);
            Console.WriteLine("Đã xóa sinh viên");
        }

        // hiển thị
        public async Task ShowAllAsync()
        {
            var students = await _jsonServices.LoadAsync();
            if (!students.Any())
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }

            Table();
            foreach (var s in students)
            {
                Console.WriteLine($"{s.Id,-5} {s.FullName,-25} {s.GPA,-10} {s.ImagePath}");
            }
        }

        // tìm theo id
        public async Task FindByIdAsync()
        {
            var students = await _jsonServices.LoadAsync();

            Console.Write("Nhập Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                return;
            }

            var s = students.FirstOrDefault(x => x.Id == id);
            if (s == null)
            {
                Console.WriteLine($"Không tìm thấy sinh viên có ID là {id}");
                return;
            }

            Table();
            Console.WriteLine($"{s.Id, -5} {s.FullName, -25} {s.GPA, -10} {s.ImagePath}");
        }

        // gpa >= 8
        public async Task ListGpaAbove8Async()
        {
            var students = await _jsonServices.LoadAsync();
            var result = students.Where(s => s.GPA >= 8).ToList();

            if (!result.Any())
            {
                Console.WriteLine("Không có sinh viên nào có GPA >= 8");
                return;
            }

            Table();
            foreach (var s in result)
            {
                Console.WriteLine($"{s.Id,-5} {s.FullName,-25} {s.GPA,-10} {s.ImagePath}");
            }
        }

        // top gpa
        public async Task TopGpaAsync()
        {
            var students = await _jsonServices.LoadAsync();

            var top = students
                .OrderByDescending(s => s.GPA)
                .Take(5)
                .ToList();

            if (!top.Any())
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }

            Console.WriteLine("Danh sách top 5 sinh viên có GPA cao nhất:");
            Table();
            foreach (var s in top)
            {
                Console.WriteLine($"{s.Id,-5} {s.FullName,-25} {s.GPA,-10} {s.ImagePath}");
            }
        }
    }
}
