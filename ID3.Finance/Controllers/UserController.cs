using ID3.Finance.Auth.Entities;
using ID3.Finance.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Runtime.InteropServices;
using System.Text;

namespace ID3.Finance.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Region = model.Region
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    return Ok(identityResult);
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return BadRequest();
                }

            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn(UserSigninModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (signInResult.Succeeded)
                {
                    
                    return Ok();
                }
                
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet("message")]
        public IActionResult Message()
        {
            string username = HttpContext.User.Identity.Name;

            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "user", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = username;
            var byteMessage = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "user", basicProperties: null, body: byteMessage);

            return Ok();
        }
    }
}
