using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoServer.Models;


namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly API_Service api = new API_Service();

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        API_User user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    Account accounts = api.GetBalance();
                    Console.WriteLine("Your current balance is: " + accounts.Balance);

                }
                else if (menuSelection == 2)
                {
                    Transfer transaction = new Transfer();
                    int id = UserService.GetUserId();
                    List<Transfer> transfers = api.AllTransactionsById(id);
                    foreach(Transfer transfer in transfers)
                    {
                        Console.WriteLine(transfer.AccountFrom + " " +  transfer.accountTo + " " + transfer.Amount + " " + transfer.TransferId);
                    }
                    int details = CLIHelper.GetInteger("Enter the ID of the transaction you would like to view. (0 to cancel)");
                    transaction = api.TransactionDetails(details);
                    if(details == 0 || transaction == null)
                    {
                        MenuSelection();
                    }
                    else if(details == transaction.TransferId)
                    {
                        int transactionId = transaction.TransferId;

                        Console.WriteLine("id: " + transaction.TransferId);
                        Console.WriteLine("From: " + transaction.AccountFrom);
                        Console.WriteLine("To: " + transaction.accountTo);
                        Console.WriteLine("Type: " + transaction.TransferTypeId);
                        Console.WriteLine("Status: " + transaction.TransferStatusId);
                        Console.WriteLine("Amount: " + transaction.Amount);
                    }
                }
                else if (menuSelection == 3)
                {

                }
                else if (menuSelection == 4)
                {
                    List<User> newUser = api.GetAllUsers();
                    foreach (User users in newUser)
                    {
                        Console.WriteLine(users.UserId.ToString() + " " +  users.Username);
                    }
                    Program.Transaction();
                 

                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    Run(); //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }

        private static void Transaction()
        {
            Account account = new Account();
            Transfer transfers = new Transfer();

            try
            {
                int recipient = CLIHelper.GetInteger("Enter id of user you are trying to send to: ");
                account = api.GetAccounts(recipient);

                if (recipient == 0 || account == null)
                {
                    MenuSelection();
                }
                else
                {
                    decimal amount = CLIHelper.GetDecimal("Enter amount: ");

                    if (amount > api.GetBalance().Balance)
                    {
                        Console.WriteLine("Insufficient funds. Please try again.");
                        Transaction();
                    }
                    else
                    {
                        Account userAccount = new Account();
                        int id = UserService.GetUserId();
                        transfers.AccountFrom = id;
                        transfers.accountTo = recipient;
                        transfers.Amount = amount;
                        api.Transactions(transfers);
                        api.UpdateBalace(transfers);
                        Console.WriteLine("Your transfer has been approved!");
                        MenuSelection();
                    }
                }
            }catch(Exception e)
            {

            }
        }
    }
}
