using System.Text.Json;

namespace SmartAssignmentTracker
{
    public static class StudentStorage
    {
        private static readonly string FilePath = "student.json";

        public static Student Load()
        {
            if (!File.Exists(FilePath))
                return new Student();

            string json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
                return new Student();

            return JsonSerializer.Deserialize<Student>(json) ?? new Student();
        }

        public static void Save(Student student)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(student, options);
            File.WriteAllText(FilePath, json);
        }
    }
}
