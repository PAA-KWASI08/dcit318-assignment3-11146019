namespace Question4_SchoolGrading
{
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }
        public Student(int id, string name, int score) => (Id, FullName, Score) = (id, name, score);
        public string GetGrade() => Score >= 80 ? "A" : Score >= 70 ? "B" : Score >= 60 ? "C" : Score >= 50 ? "D" : "F";
    }

    public class InvalidScoreFormatException : Exception { public InvalidScoreFormatException(string msg) : base(msg) { } }
    public class MissingFieldException : Exception { public MissingFieldException(string msg) : base(msg) { } }

    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();
            using var reader = new StreamReader(inputFilePath);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length != 3) throw new MissingFieldException($"Missing fields in line: {line}");
                if (!int.TryParse(parts[0], out var id)) throw new InvalidScoreFormatException("Invalid ID format");
                if (!int.TryParse(parts[2], out var score)) throw new InvalidScoreFormatException("Invalid score format");
                students.Add(new Student(id, parts[1].Trim(), score));
            }
            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath);
            foreach (var s in students)
                writer.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
        }
    }
}