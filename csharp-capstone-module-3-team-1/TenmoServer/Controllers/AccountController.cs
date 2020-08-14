using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO _accountsdao;
        private readonly IUserDAO _userdao;
        private readonly ITransferDAO _transferdao;
        public AccountController(IAccountDAO accountsDAO, IUserDAO userDAO, ITransferDAO transferDAO)
        {
            if(_accountsdao == null)
            {
                _accountsdao = accountsDAO;
            }
            if (_userdao == null)
            {
                _userdao = userDAO;
            }
            if (_transferdao == null)
            {
                _transferdao = transferDAO;
            }
        }
        [HttpGet("/users/transfers/{id}")]
        public Accounts GetAccounts(int id)
        {
            Accounts accounts = _accountsdao.GetAccounts(id);
            return accounts;
        }

        [HttpGet]
        public Accounts GetBalance(string username)
        {
            User user = _userdao.GetUser(username);
            Accounts accounts = new Accounts();
            accounts.Balance = _userdao.GetBalance(user);
            return accounts;
        }

        [HttpGet("users")]
        public List<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            allUsers = _userdao.GetUsers();
            return allUsers;
        }

        [HttpGet("users/{id}")]
        public Accounts GetUsersById(int id)
        {
            Accounts accounts = _accountsdao.GetAccounts(id);
            return accounts;
        }

        [HttpPost("users/transfers")]
        public ActionResult<Transfer> Transaction(Transfer transfers)
        {
            Transfer transfersAdded = _transferdao.Transaction(transfers);
            return Ok();
        }
        [HttpGet("users/transfers/{id}")]
        public List<Transfer> AllTransactionsById(int id)
        {
            List<Transfer> transfers = new List<Transfer>();
            transfers = _transferdao.AllTransactionById(id);
            return transfers;
        }
        //[HttpPost("/users/transfers")]
        //public ActionResult<Transfer> Transaction(Transfer transfers)
        //{
        //    Transfer transferAdded = _transferdao.Transaction(transfers);
        //    return Ok();
        //}
        [HttpPut]
        public ActionResult<Accounts> UpdateBalance(Transfer transfers)
        {
            Accounts accountUpdated = _accountsdao.UpdateBalance(transfers);
            return Ok();
        }
        [HttpGet("users/transfers/details/{id}")]
        public Transfer TransactionsDetails(int id)
        {
            Transfer transfers = _transferdao.TransactionDetails(id);
            return transfers;
        }

      


    }
}