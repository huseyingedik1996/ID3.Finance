using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ID3.Finance.Binance.Models;
using ID3.Finance.Binance.Models.BinanceModels;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Newtonsoft.Json;
using RabbitMQ.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static ID3.Finance.Binance.Models.BinanceModels.BinanceKlineResultModel;

namespace ID3.Finance.Binance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinanceController : ControllerBase
    {

        private readonly BinanceContext _context;


        private const string RapidApiKey = "dc74fb1479msh714dd5441b9747ep1fc44ejsn02d82de659c8";
        private const string RapidApiHost = "binance43.p.rapidapi.com";



        public BinanceController(BinanceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBinanceData()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://binance43.p.rapidapi.com/klines?symbol=BTCUSDT&interval=5m&limit=1"),
                };
                request.Headers.Add("X-RapidAPI-Key", RapidApiKey);
                request.Headers.Add("X-RapidAPI-Host", RapidApiHost);

                try
                {
                    
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        List<object[]> parsedData = JsonConvert.DeserializeObject<List<object[]>>(body);
                       
                        string acilisFiyati = (string)parsedData[0][1];
                        string enYuksekFiyat = (string)parsedData[0][2];
                        string enDusukFiyat = (string)parsedData[0][3];
                        string kapanisFiyati = (string)parsedData[0][4];
                        string hacimAgirlikliOrtalamaFiyat = (string)parsedData[0][7];
                        string temelParaHacmi = (string)parsedData[0][9];
                        string karsiParaHacmi = (string)parsedData[0][10];

                        BinanceKlineResultModel binance = new()
                        {
                            
                            Open = acilisFiyati,
                            High = enYuksekFiyat,
                            Low = enDusukFiyat,
                            Close = kapanisFiyati,
                            WeightedAveragePrice = hacimAgirlikliOrtalamaFiyat,
                            BaseAssetVolume = temelParaHacmi,
                            QuoteAssetVolume = karsiParaHacmi
                        };
                        await _context.AddAsync(binance);
                        await _context.SaveChangesAsync();
                        return Ok(body);
                    }


                }
                catch (HttpRequestException ex)
                {
                    return BadRequest("HTTP request error: " + ex.Message);
                }
            }
        }
        [HttpGet("avarage")]
        public async Task<IActionResult> GetAvarage()
        {
            var last12Records = await _context.KlineResults.OrderByDescending(x => x.Id).Take(12).ToListAsync();
            var last1 = await _context.KlineResults.OrderByDescending(x => x.Id).Take(1).ToListAsync();
           

            double sumHigh = last12Records.Sum(record => Convert.ToDouble(record.High));
            double sumLow = last12Records.Sum(record => Convert.ToDouble(record.Low));
            double sumWeg = last12Records.Sum(record => Convert.ToDouble(record.WeightedAveragePrice));



            int count = last12Records.Count;
            double averageHigh = sumHigh / count;
            double averageLow = sumLow / count;
            
            

            return Ok(new { Average = averageHigh, Low = averageLow });

        }

        [HttpGet("predicate1Hour")]
        public async Task<IActionResult> GetPredicate1Hour()
        {
            var last12Records = await _context.KlineResults.OrderByDescending(x => x.Id).Take(12).ToListAsync();
            

            double sumHigh = last12Records.Sum(record => Convert.ToDouble(record.High));
            double sumLow = last12Records.Sum(record => Convert.ToDouble(record.Low));
            double sumWeg = last12Records.Sum(record => Convert.ToDouble(record.WeightedAveragePrice));
            double sumOpen = last12Records.Sum(record => Convert.ToDouble(record.Open));
            double sumClose = last12Records.Sum(record => Convert.ToDouble(record.Close));


            int count = last12Records.Count;
            double averageHigh = sumHigh / count;
            double averageLow = sumLow / count;
            double averageOpen = sumOpen / count;
            double averageClose = sumClose / count;
            double avarageWeg = sumWeg / count;

            var result1 = (averageHigh + averageLow) / 2;
            

            var final = result1 / avarageWeg;

            if (averageClose > averageOpen)
            {
                var result = averageClose * (100 + final) / 100;
                
                return Ok(new { final = final, predicate = result});
            }
            else
            {
                var result = averageClose * (100 - final) / 100;
                return Ok(new { final = final, predicate = result});
            }
        }

        [HttpGet("predicate15Min")]
        public async Task<IActionResult> GetPredicate15Min()
        {
            var last3Records = await _context.KlineResults.OrderByDescending(x => x.Id).Take(3).ToListAsync();


            double sumHigh = last3Records.Sum(record => Convert.ToDouble(record.High));
            double sumLow = last3Records.Sum(record => Convert.ToDouble(record.Low));
            double sumWeg = last3Records.Sum(record => Convert.ToDouble(record.WeightedAveragePrice));
            double sumOpen = last3Records.Sum(record => Convert.ToDouble(record.Open));
            double sumClose = last3Records.Sum(record => Convert.ToDouble(record.Close));


            int count = last3Records.Count;
            double averageHigh = sumHigh / count;
            double averageLow = sumLow / count;
            double averageOpen = sumOpen / count;
            double averageClose = sumClose / count;
            double avarageWeg = sumWeg / count;

            var result1 = (averageHigh + averageLow) / 2;


            var final = result1 / avarageWeg;

            if (averageClose > averageOpen)
            {
                var result = averageClose * (100 + final) / 100;

                return Ok(new { final = final, predicate = result });
            }
            else
            {
                var result = averageClose * (100 - final) / 100;
                return Ok(new { final = final, predicate = result });
            }
        }

        [HttpGet("getLast")]
        public async Task<IActionResult> GetLast()
        {
            var getLast = await _context.KlineResults.OrderByDescending(x => x.Id).Take(1).ToListAsync();
            
            return Ok(getLast);
        }
        [HttpGet("message")]
        public IActionResult Message()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "binance", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = _context.KlineResults.OrderByDescending(x => x.Id).FirstOrDefault();
            var messageId = message.Close.ToString(); 
            var byteMessage = Encoding.UTF8.GetBytes(messageId);

            channel.BasicPublish(exchange: "", routingKey: "binance", basicProperties: null, body: byteMessage);

            return Ok();
        }
    }
}
