using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoServer.Models;

namespace TenmoClient
{
    public class API_Service
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly static string ACCOUNT_URL = API_BASE_URL + "account";
        private readonly IRestClient client;
        private static API_User user = new API_User();
        private static Transfer transfer = new Transfer();
        //private static LoginUser newUser = new LoginUser();

        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }

        public string UNAUTHORIZED_MSG { get { return "Authorization is required for this endpoint. Please log in."; } }
        public string FORBIDDEN_MSG { get { return "You do not have permission to perform the requested action"; } }
        public string OTHER_4XX_MSG { get { return "Error occurred - received non-success response: "; } }

        public API_Service()
        {
            client = new RestClient();
        }

        public API_Service(IRestClient restClient)
        {
            client = restClient;
        }

        public Account GetBalance()
        {
            Account accounts = new Account();
            RestRequest request = new RestRequest(ACCOUNT_URL);
            request.AddParameter("username", UserService.GetUser().Username);
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            RestRequest request = new RestRequest(ACCOUNT_URL + "/users");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public Account GetAccounts(int id)
        {
            Account accounts = new Account();
            RestRequest request = new RestRequest(ACCOUNT_URL + "/users/" + id);
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public Transfer Transactions(Transfer transfers)
        {
            User user = new User();
            RestRequest request = new RestRequest(ACCOUNT_URL + "/users/transfers");
            request.AddJsonBody(transfers);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public List<Transfer> AllTransactionsById(int id)
        {
            List<Transfer> transfers = new List<Transfer>();
            RestRequest request = new RestRequest(ACCOUNT_URL + "/users/transfers/" + id);
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public Account UpdateBalace(Transfer transfers)
        {
            RestRequest request = new RestRequest(ACCOUNT_URL);
            request.AddJsonBody(transfers);

            IRestResponse<Account> response = client.Put<Account>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public Transfer TransactionDetails(int id)
        {
            Transfer transfers = new Transfer();
            RestRequest request = new RestRequest(ACCOUNT_URL + "/users/transfers/details/" + id);
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
       

        public string ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                return "Error occurred - unable to reach server.";
            }
            else if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return UNAUTHORIZED_MSG;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return FORBIDDEN_MSG;
                }
                return OTHER_4XX_MSG + (int)response.StatusCode;
            }
            return "";
        }
    }
}
