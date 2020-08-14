using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfer Transaction(Transfer transfers)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("insert into transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) values (2, 2, @accountfrom, @accountto, @amount)", conn);
                    cmd.Parameters.AddWithValue("@accountfrom", transfers.AccountFrom);
                    cmd.Parameters.AddWithValue("@accountto", transfers.accountTo);
                    cmd.Parameters.AddWithValue("@amount", transfers.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
               
            }
            return transfers;
        }
        public List<Transfer> AllTransactionById(int id)
        {
            List<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE account_from = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transfer a = GetTransfersFromReader(reader);
                            transfers.Add(a);
                        }

                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;
        }

        public Transfer TransactionDetails(int id)
        {
            Transfer transfers = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE transfer_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                       transfers = GetTransfersFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;

        }
        private Transfer GetTransfersFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                accountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToInt32(reader["amount"]),
            };
            return t;
        }
    }
}
