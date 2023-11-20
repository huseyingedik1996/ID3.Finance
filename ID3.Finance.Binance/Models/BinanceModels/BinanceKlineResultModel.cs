namespace ID3.Finance.Binance.Models.BinanceModels
{
    public class BinanceKlineResultModel
    {
        public int Id { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string WeightedAveragePrice { get; set; }
        public string BaseAssetVolume { get; set; }
        public string QuoteAssetVolume { get; set; }
    }
}
