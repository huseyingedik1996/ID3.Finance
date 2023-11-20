namespace ID3.Finance.Portfolio.Models
{
    public class PortfolioModel
    {
        public int Id { get; set; }

        public string Close { get; set; }

        public UserModel UserModel { get; set; }

        public int UserId { get; set; }
    }
}
