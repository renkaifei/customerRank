using CustomerRank.App;
using CustomerRank.Dtos;
using CustomerRank.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardChannel _channel;
        private readonly Leaderboard _leaderboard;
        private readonly CustomerSet _customerSet;
        public LeaderboardController(LeaderboardChannel channel,
            Leaderboard leaderboard,
            CustomerSet customerSet)
        {
            _channel = channel;
            _leaderboard = leaderboard;
            _customerSet = customerSet;
        }

        /// <summary>
        /// Update Customer Score
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost]
        [Route("/customer/{customerId}/score/{score}")]
        public CustomResponse<Customer> UpdateScore([FromRoute]long customerId, [FromRoute]decimal score)
        {
            CustomResponse<Customer> response = new CustomResponse<Customer>();
            try
            {
                if (customerId <= 0)
                {
                    throw new ArgumentException("CustomerId must be positive");
                }
                if (score < -1000 || score > 1000)
                {
                    throw new ArgumentException("Score must between -1000 and 1000");
                }

                Customer customer = new Customer();
                customer.CustomerId = customerId;
                customer.ScoreValue = score;
                _channel.Write(customer);
                response.Data = customer;
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("/leaderboard")]
        public CustomResponse<List<Customer>> GetCustomers([FromQuery]int start,[FromQuery]int end) 
        {
            CustomResponse<List<Customer>> response = new CustomResponse<List<Customer>>();
            try
            {
                if (start <= 0) throw new ArgumentException("start rank must be positive");
                if (end <= 0) throw new ArgumentException("end rank must be positive");
                if (start > end) throw new ArgumentException("start rank must be low than end");

                List<Customer> customers = _leaderboard.GetCustomers(start, end);
                response.Data = customers;
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("/leaderboard/{customerId}")]
        public CustomResponse<List<Customer>> GetCustomersByCustomerId([FromRoute]long customerId,
            [FromQuery]int high, [FromQuery]int low)
        {
            CustomResponse<List<Customer>> response = new CustomResponse<List<Customer>>();
            try
            {
                Customer? customer = _customerSet.GetCustomer(customerId);
                if (customer == null) throw new ArgumentException("customer not exists");
                response.Data = _leaderboard.GetCustomers(customer!, high, low);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
