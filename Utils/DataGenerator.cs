using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Utils
{
    public class DataGenerator
    {
        private List<Student> _students = new();
        private readonly JsonServices _jsonServices;

        public static string GetProjectRoot()
        {
            return Directory
                .GetParent(AppContext.BaseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .FullName;
        }

        public DataGenerator(List<Student> students, JsonServices jsonServices)
        {
            _students = students;
            _jsonServices = jsonServices;
        }

        public enum LastName
        {
            Nguyễn, Trần, Lê, Phạm, Hoàng, Huỳnh, Phan, Vũ, Võ
        }

        public enum MiddleName
        {
            Ngọc, Duy, Xuân, Văn, Hùng, Minh, Quân,
        }

        public enum FirstName
        {
            Huy, Hoàng, Bảo, Quân, Khang, Khoa, Long, Hiệp, Dũng, Bình, Tài, Tuấn
        }

        public static T RandomEnum<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Shared.Next(values.Length))!;
        }

        public static string RandomFullName()
        {
            var lastName = RandomEnum<LastName>();
            var middleName = RandomEnum<MiddleName>();
            var firstName = RandomEnum<FirstName>();

            return $"{lastName} {middleName} {firstName}";
        }


        public async Task GenerateStudent(int cnt)
        {
            var projectRoot = GetProjectRoot();
            var imageDir = Path.Combine(projectRoot, "Data", "images");
            Directory.CreateDirectory(imageDir);

            var students = await _jsonServices.LoadAsync();
            int startId = students.Any() ? students.Max(s => s.Id) + 1 : 1;

            for (int i = 0; i < cnt; i++)
            {
                int id = startId + i;
                var fileName = $"student_{id}.png";

                var absoluteImagePath = Path.Combine(imageDir, fileName);
                using FileStream fs = File.Create(absoluteImagePath);

                var relativeImagePath = Path.Combine("Data", "images", fileName);

                var tmp = new Student(
                    id,
                    RandomFullName(),
                    Math.Round(Random.Shared.NextDouble() * 9 + 1, 2),
                    relativeImagePath
                );

                students.Add(tmp);
            }

            await _jsonServices.SaveAsync(students);
            Console.WriteLine($"Tạo thành công {cnt} sinh viên");
        }


        //public static async Task Main(string[] args)
        //{
        //    var generator = new DataGenerator();
        //    await generator.GenerateStudent(1000);
        //}
    }
}
