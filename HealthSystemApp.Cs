namespace Question2_HealthSystemApp
{
    public class Repository<T>
    {
        private readonly List<T> items = new();
        public void Add(T item) => items.Add(item);
        public List<T> GetAll() => items.ToList();
        public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);
        public bool Remove(Func<T, bool> predicate)
        {
            var found = items.FirstOrDefault(predicate);
            if (found == null) return false;
            items.Remove(found);
            return true;
        }
    }

    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }
        public Patient(int id, string name, int age, string gender) => (Id, Name, Age, Gender) = (id, name, age, gender);
        public override string ToString() => $"{Name} (ID: {Id}) Age:{Age} Gender:{Gender}";
    }

    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }
        public Prescription(int id, int patientId, string med, DateTime date) => (Id, PatientId, MedicationName, DateIssued) = (id, patientId, med, date);
        public override string ToString() => $"{MedicationName} on {DateIssued:d}";
    }

    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private readonly Dictionary<int, List<Prescription>> _map = new();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alice Smith", 30, "Female"));
            _patientRepo.Add(new Patient(2, "Bob Johnson", 45, "Male"));
            _prescriptionRepo.Add(new Prescription(101, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 2, "Paracetamol", DateTime.Now.AddDays(-5)));
        }

        public void BuildMap()
        {
            foreach (var p in _prescriptionRepo.GetAll())
            {
                if (!_map.ContainsKey(p.PatientId)) _map[p.PatientId] = new();
                _map[p.PatientId].Add(p);
            }
        }

        public void PrintPatients()
        {
            foreach (var p in _patientRepo.GetAll()) Console.WriteLine(p);
        }

        public void PrintPrescriptionsFor(int id)
        {
            if (_map.TryGetValue(id, out var list))
                foreach (var pr in list) Console.WriteLine(pr);
        }
    }

    public class Program
    {
        public static void Main()
        {
            var app = new HealthSystemApp();
            app.SeedData();
            app.BuildMap();
            app.PrintPatients();
            app.PrintPrescriptionsFor(1);
        }
    }
}