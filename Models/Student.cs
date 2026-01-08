namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public double GPA { get; set; }
        public string? ImagePath { get; set; }

        public Student()
        {
            
        }

        public Student(int id, string fullName, double gpa, string imagePath)
        {
            Id = id;
            FullName = fullName;
            GPA = gpa;
            ImagePath = imagePath;
        }
    }
}
