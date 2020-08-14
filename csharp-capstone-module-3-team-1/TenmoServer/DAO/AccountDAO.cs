using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private readonly string connectionString;
        const decimal startingBalance = 1000;
        //private double accountBalance = 0;
        public AccountDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

      public Accounts GetAccounts(int id)
        {
            Accounts returnAccounts = new Accounts(); 

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE user_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Accounts a = GetAccountsFromReader(reader);
                            return a;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnAccounts;
        }
        public Accounts UpdateBalance(Transfer transfers)
        {
            Accounts returnAccounts = new Accounts();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("update accounts set balance = (balance-@amount) where user_id = @userid update accounts set balance = (balance+@amount) where user_id = @recipientid", conn);
                    cmd.Parameters.AddWithValue("@userid", transfers.AccountFrom);
                    cmd.Parameters.AddWithValue("@amount", transfers.Amount);
                    cmd.Parameters.AddWithValue("@recipientid", transfers.accountTo);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnAccounts;

        }
        private Accounts GetAccountsFromReader(SqlDataReader reader)
        {
            Accounts a = new Accounts()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };

            return a;
        }
        
        
    }
}
