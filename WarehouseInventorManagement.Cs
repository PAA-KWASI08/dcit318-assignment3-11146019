namespace Question3_WarehouseInventory
{
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }
        public ElectronicItem(int id, string name, int qty, string brand, int warranty) => (Id, Name, Quantity, Brand, WarrantyMonths) = (id, name, qty, brand, warranty);
        public override string ToString() => $"{Name} ({Brand}), Qty: {Quantity}, Warranty: {WarrantyMonths} months";
    }

    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }
        public GroceryItem(int id, string name, int qty, DateTime expiry) => (Id, Name, Quantity, ExpiryDate) = (id, name, qty, expiry);
        public override string ToString() => $"{Name}, Qty: {Quantity}, Expires: {ExpiryDate:d}";
    }

    public class DuplicateItemException : Exception { public DuplicateItemException(string msg) : base(msg) { } }
    public class ItemNotFoundException : Exception { public ItemNotFoundException(string msg) : base(msg) { } }
    public class InvalidQuantityException : Exception { public InvalidQuantityException(string msg) : base(msg) { } }

    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();
        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id)) throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            _items[item.Id] = item;
        }
        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id)) throw new ItemNotFoundException($"Item ID {id} not found.");
            return _items[id];
        }
        public void RemoveItem(int id)
        {
            if (!_items.Remove(id)) throw new ItemNotFoundException($"Item ID {id} not found.");
        }
        public List<T> GetAllItems() => _items.Values.ToList();
        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0) throw new InvalidQuantityException("Quantity cannot be negative.");
            if (!_items.ContainsKey(id)) throw new ItemNotFoundException($"Item ID {id} not found.");
            _items[id].Quantity = newQuantity;
        }
    }

    public class WarehouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 10, "Samsung", 12));
            _groceries.AddItem(new GroceryItem(1, "Apples", 20, DateTime.Now.AddDays(10)));
            _groceries.AddItem(new GroceryItem(2, "Bread", 15, DateTime.Now.AddDays(3)));
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems()) Console.WriteLine(item);
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var current = repo.GetItemById(id);
                repo.UpdateQuantity(id, current.Quantity + quantity);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try { repo.RemoveItem(id); } catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void RunDemo()
        {
            SeedData();
            PrintAllItems(_groceries);
            PrintAllItems(_electronics);
            try { _electronics.AddItem(new ElectronicItem(1, "Tablet", 7, "Apple", 18)); } catch (Exception ex) { Console.WriteLine(ex.Message); }
            RemoveItemById(_groceries, 99);
            try { _groceries.UpdateQuantity(1, -5); } catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
