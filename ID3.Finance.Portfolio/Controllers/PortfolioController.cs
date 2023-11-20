using ID3.Finance.Portfolio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ID3.Finance.Portfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly PortDbContext _context;

        public PortfolioController(PortDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUserMessages()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var userconsumer = new EventingBasicConsumer(channel);
          


            userconsumer.Received += (model, ea) =>
            {
                var byteMessage = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(byteMessage);
                var ts = message.ToString();

                _context.UserModel.Add(new UserModel
                {
                    UserName = ts
                });
                _context.SaveChanges(); 
            };
            
            


            var user = channel.BasicConsume(queue: "user", autoAck: false, consumer: userconsumer);
            

            return Ok();
        }

        [HttpGet("binance")]
        public IActionResult GetBinanceMessages()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var binanceconsumer = new EventingBasicConsumer(channel);



            binanceconsumer.Received += (model, ea) =>
            {
                var byteMessage = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(byteMessage);

            };

            var user = channel.BasicConsume(queue: "binance", autoAck: false, consumer: binanceconsumer);

            return Ok();
        }
    }
}
