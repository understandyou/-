using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace jwtServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 注释
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        public IActionResult shouquan()
        {

            
            //对此密钥
            var securkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Sceret));
            //加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signing = new SigningCredentials(securkey, algorithm);
            //登录信息
            var claims = new[]
            {
                new Claim("family","zf")
                ,new Claim("fq","bb"), 
                new Claim("mq","mm"), 
                //jwt的类型
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"), 
            };
            //用户身份
            var token = new JwtSecurityToken(Constants.Issuer,Constants.Audiance,claims,DateTime.Now,DateTime.Now.AddDays(1), signing);
            var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new {access_token = jsonToken });
        }
    }
}
