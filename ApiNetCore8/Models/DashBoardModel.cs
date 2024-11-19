namespace ApiNetCore8.Models
{
    public class DashBoardModel
    {
        public DateTime DateTime { get; set; }

        public int QuantityProductIn {  get; set; }

        public int QuantityProductOut { get; set; }

        public decimal TotalPriceIn { get; set; }

        public decimal TotalPriceOut { get; set; }
    }
}
