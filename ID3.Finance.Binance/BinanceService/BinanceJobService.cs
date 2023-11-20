using ID3.Finance.Binance.Models;
using ID3.Finance.Binance.Models.BinanceModels;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ID3.Finance.Binance.BinanceService
{
    [DisallowConcurrentExecution]
    public class BinanceJobService : IJob
    {
        private readonly BinanceContext _context;
        private const string RapidApiKey = "dc74fb1479msh714dd5441b9747ep1fc44ejsn02d82de659c8";
        private const string RapidApiHost = "binance43.p.rapidapi.com";

        public BinanceJobService(BinanceContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
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
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception("HTTP request error: " + ex.Message);
                }
            }
        }
    }
}
