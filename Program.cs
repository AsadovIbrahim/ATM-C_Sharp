using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace ATM_C_
{
    internal class Program
    {
        public class BankCard
        {
            public string BankName { get; set; }
            public string FullName { get; set; }
            public string PAN { get; set; }
            public string PIN { get; set; }
            public string CVC { get; set; }
            public string ExpireDate { get; set; }
            public decimal Balance { get; set; }

            public BankCard(string bankName,string fullName,string pan,string pin,string cvc,
                string expireDate,decimal balance)
            {
                BankName = bankName;
                FullName = fullName;
                PAN = pan;
                PIN = pin;
                CVC = cvc;
                ExpireDate = expireDate;
                Balance = balance;

            }

            
        } 
        public class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public decimal Salary { get; set; } 

            public BankCard BankAccount { get; set; }
            public User(int id,string name,string surname,int age,decimal salary,BankCard bankAccount) {
                
                ID = id;
                Name = name;
                Surname = surname;
                Age= age;
                Salary = salary;
                BankAccount = bankAccount;
                
            }
            
        }

        public class Bank
        {
            public List<User> Clients { get; set; }
            public Bank()
            {
                Clients = new List<User>();
            }

            public void showCardBalance(BankCard bankCard)
            {
                Console.WriteLine($"Balance:{bankCard.Balance}");
            }
        }

        public void WithDrawCash(BankCard bankCard)
        {
            Console.WriteLine("How much cash do you want to withdraw?:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            if (amount > bankCard.Balance)
            {
                throw new Exception("Insufficient Funds");
            }
            bankCard.Balance -= amount;
            Console.WriteLine($"{amount}manat: were successfully deducted from your balance\n" +
                $"New Balance:{bankCard.Balance} ");
        }
        private List<Transaction> transactionHistory = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            transactionHistory.Add(transaction);
        }
        

        public static void ShowTransactions()
        {
            Console.WriteLine($"Transaction Date:{DateTime.Now}");
            Console.WriteLine($"Kartin Tipi:{DataType.CreditCard}");
        }
        public void TransferFunds(BankCard sourceCard,BankCard destinationCard,decimal amount)
        {
            if (sourceCard.PIN != destinationCard.PIN)
            {
                Console.WriteLine("The PIN code entered does not match any card.");
                return;
            }
            if (amount > sourceCard.Balance)
            {
                throw new Exception("Insufficient Funds");
            }
            sourceCard.Balance -= amount;
            destinationCard.Balance += amount;

            Console.WriteLine($"New Balance for {sourceCard.FullName}:{sourceCard.Balance}");
            Console.WriteLine($"New Balance for {destinationCard.FullName}:{destinationCard.Balance}");
        }

        static  void Main(string[] args) 
        {
            BankCard bankCard = new BankCard("Capital Bank", "Ibrahim Asadov", "5103071506577648", "1234", "123", "07/2025", 2000);
            BankCard bankCard2 = new BankCard("Capital Bank", "Rustem Hesenli", "5663767647647778", "12345", "123", "07/2027", 1000);
            User user = new User(1, "Ibrahim", "Asadov", 18, 2000, bankCard);
            User user2= new User(2, "Rustem", "Hesenli", 16, 1000, bankCard2);

            Bank bank = new Bank();
            bank.Clients.Add(user);

            while (true){
                Console.ForegroundColor= ConsoleColor.Green; 
                Console.WriteLine("Please Enter Your PIN:");
                string enteredPin = Console.ReadLine();

                if (enteredPin == user.BankAccount.PIN)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Xos Gelmisiniz:{user.Name} {user.Surname}");
                    Console.WriteLine("Zehmet olmasa asagidakilardan birini secin:");
                    Console.WriteLine("1.Balansi Gosterin");
                    Console.WriteLine("2.Nagd Pul");
                    Console.WriteLine("3.Edilen Emeliyyatlarin Siyahisi");
                    Console.WriteLine("4.Kartdan Karta Kocurme");

                    int option = Convert.ToInt32(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine($"Balansinizda {user.BankAccount.Balance} Manat var");
                            break;
                        case 2:
                            Console.WriteLine("Asagidakilardan birini secin:");
                            Console.WriteLine("\n1.10 AZN");
                            Console.WriteLine("\n2.20 AZN");
                            Console.WriteLine("\n3.50 AZN");
                            Console.WriteLine("\n4.100 AZN");
                            Console.WriteLine("\n5.Diger");
                            

                            int withdrawOption;
                            while (!int.TryParse(Console.ReadLine(), out withdrawOption) || withdrawOption < 1 || withdrawOption > 5)
                            {
                                Console.WriteLine("Yanlis Secim!!! 1-5 araliginda secim edin:");
                            }
                            decimal withdrawAmount = 0;
                           

                            switch (withdrawOption)
                            {
                                case 1:
                                    withdrawAmount = 10;
                                    break;
                                case 2:
                                    withdrawAmount = 20;
                                    break;
                                case 3:
                                    withdrawAmount = 50;
                                    break;
                                case 4:
                                    withdrawAmount = 100;
                                    break;
                                case 5:
                                    Console.WriteLine("Meblegi qeyd edin:");
                                    withdrawAmount = Convert.ToDecimal(Console.ReadLine());
                                    break;
                                default:
                                    Console.WriteLine("Duzgun Secim Edilmeyib!!!");
                                    break;
                            }
                            if (withdrawAmount > user.BankAccount.Balance)
                            {
                                try {
                                    throw new Exception("Balansinizda Kifayet Qeder Mebleg Yoxdur!!!");
                                }catch(Exception ex) { Console.WriteLine(ex.Message); 
                                }
                            }
                            else
                            {
                                user.BankAccount.Balance -= withdrawAmount;
                                Console.WriteLine($"{withdrawAmount} Manat pul balansdan ugurla cixarildi!!!\n" +
                                    $"Balansinizda {user.BankAccount.Balance} Manat mebleg var");
                                Console.WriteLine("Kartinizi Goturun:");
                                break;

                            }
                            break;
                        case 3:
                            ShowTransactions();
                            break;

                        case 4:
                            while(user.BankAccount.PAN.Length==16){
                                Console.WriteLine("Enter The PAN of the card to transfer to:");
                                string targetPAN = Console.ReadLine();

                                try
                                {
                                    
                                    if (targetPAN.Length == 16)
                                    {
                                        Console.WriteLine("Enter the amount to transfer:");
                                        decimal transferAmount = decimal.Parse(Console.ReadLine());
                                        user.BankAccount.Balance -= transferAmount;
                                        user2.BankAccount.Balance += transferAmount;
                                        Console.WriteLine($"Successfully transferred {transferAmount} Manat to {targetPAN}.\nBalansinizda:{user.BankAccount.Balance} Manat \n" +
                                            $"Kocurulen Sexsde:{user2.BankAccount.Balance} Manat");


                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong PAN");
                                    }

                                }
                                catch (InvalidOperationException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            }
                            break;
                            

                           
   
                        default:
                            Console.WriteLine("Duzgun Secim Edilmeyib!!!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Sifre Yanlisdir Yeniden Cehd Edin:");
                }
            }

        }
    }
}




