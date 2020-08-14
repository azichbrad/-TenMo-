using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<Transfer> AllTransactionById(int id);
        Transfer Transaction(Transfer transfers);
        Transfer TransactionDetails(int id);

    }
}
