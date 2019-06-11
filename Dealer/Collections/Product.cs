using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealer
{
    public class Product
    {
        public Product()
        {

        }

        public Product(int id, string name, double quantity, decimal costPrice, double inStock, decimal costPriceTotal)
        {
            this.ID = id;
            this.Name = name;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.InStock = inStock;
            this.CostPriceTotal = costPriceTotal;
        }


        public int ID
        {
            get; set;
        }

        public string Name
        {
            get; set;    
        }

        public double Quantity
        {
            get; set;
        }

        public decimal CostPrice
        {
            get; set;
        }

        public double InStock
        {
            get; set;
        }

        public decimal CostPriceTotal
        {
            get; set;
        }

    }
}
