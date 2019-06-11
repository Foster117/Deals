namespace Dealer
{
    public class Client
    {
        public Client()
        {

        }

        public Client(int id, string name, string product, double quantity, decimal price, decimal totalPrice, decimal costPrice, decimal profit, string note)
        {
            this.Id = id;
            this.Name = name;
            this.Product = product;
            this.Quantity = quantity;
            this.Price = price;
            this.TotalPrice = totalPrice;
            this.CostPrice = costPrice;
            this.Profit = profit;
            this.Note = note;
        }


        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Product
        {
            get; set;
        }
        public double Quantity
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public decimal TotalPrice
        {
            get; set;
        }
        public decimal CostPrice
        {
            get; set;
        }
        public decimal Profit
        {
            get; set;
        }
        public string Note
        {
            get; set;
        }
    }
}
