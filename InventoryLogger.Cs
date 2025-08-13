namespace Question5_InventoryLogger
{
    public interface IInventoryEntity { int Id { get; } }

    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;
        public InventoryLogger(string filePath) => _filePath = filePath;

        public void Add(T item) => _log.Add(item);
        public List<T> GetAll() => _log.ToList();
        public void SaveToFile()
        {
            try { File.WriteAllText(_filePath, System.Text.Json.JsonSerializer.Serialize(_log)); }
            catch (Exception ex) { Console.WriteLine($"Error saving: {ex.Message}"); }
        }
        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var content = File.ReadAllText(_filePath);
                    var items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(content);
                    if (items != null) { _log.Clear(); _log.AddRange(items); }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error loading: {ex.Message}"); }
        }
    }
}