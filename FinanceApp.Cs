// === Question 1: FinanceApp ===
namespace Question1_FinanceApp
{
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor { void Process(Transaction transaction); }

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction) =>
            Console.WriteLine($"[BankTransfer] {transaction.Category}: {transaction.Amount:C}");
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction) =>
            Console.WriteLine($"[MobileMoney] {transaction.Category}: {transaction.Amount:C}");
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction) =>
            Console.WriteLine($"[CryptoWallet] {transaction.Category}: {transaction.Amount:C}");
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }
        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }
        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"New Balance: {Balance:C}");
        }
    }

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) : base(accountNumber, initialBalance) { }
        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }
            base.ApplyTransaction(transaction);
        }
    }

    public class FinanceApp
    {
        public void Run()
        {
            var account = new SavingsAccount("SA1001", 1000m);
            var t1 = new Transaction(1, DateTime.Now, 120, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 250, "Utilities");
            var t3 = new Transaction(3, DateTime.Now, 400, "Entertainment");

            new MobileMoneyProcessor().Process(t1);
            account.ApplyTransaction(t1);
            new BankTransferProcessor().Process(t2);
            account.ApplyTransaction(t2);
            new CryptoWalletProcessor().Process(t3);
            account.ApplyTransaction(t3);
        }
    }

    public class Program
    {
        public static void Main()
        {
            new FinanceApp().Run();
        }
    }
}
