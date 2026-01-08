using System.Text.Json;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class JsonServices
    {
        private readonly string _filePath;

        public JsonServices(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<Student>> LoadAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Student>();
            }

            using FileStream fs = File.OpenRead(_filePath);

            var students = await JsonSerializer.DeserializeAsync<List<Student>>(fs);

            return students ?? new List<Student>();
        }

        public async Task SaveAsync(List<Student> students)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            using FileStream fs = File.Create(_filePath);

            var opt = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            await JsonSerializer.SerializeAsync(fs, students, opt );
        }
    }
}
