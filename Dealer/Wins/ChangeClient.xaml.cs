using System;
using System.Windows;
using System.Text.RegularExpressions;


namespace Dealer
{
    /// <summary>
    /// Interaction logic for ChangeClient.xaml
    /// </summary>
    public partial class ChangeClient : Window
    {
        Clients clients;
        Products products;
        const string pattern = @"^[0-9]{1,}($|\.?[0-9]{1,2}$)";
        int id;
        double oldQuantity;

        //Ctors
        public ChangeClient()
        {
            InitializeComponent();
        }
        public ChangeClient(Clients clients, Products products, Client client)
            : this()
        {
            this.clients = clients;
            this.products = products;
            this.id = client.Id;
            this.oldQuantity = client.Quantity;

            //Adding values to ComboBoxes
            AddValuesToNamesBox();
            AddValuesToProductsBox();
            AddValuesToNotesBox();

            changeDealName.SelectedItem = client.Name;
            changeDealProduct.SelectedItem = client.Product;
            changeDealQuantity.Text = client.Quantity.ToString();
            changeDealPrice.Text = client.Price.ToString();
            changeDealNote.Text = client.Note.ToString();
        }



        //OK Button
        private void changeClientButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (changeDealName.Text != "" && changeDealProduct.Text != "" && changeDealNote.Text != "" && changeDealPrice.Text != "" && changeDealQuantity.Text != "")
            {
                if (Regex.IsMatch(changeDealQuantity.Text, pattern) && Regex.IsMatch(changeDealPrice.Text, pattern))
                {
                    clients.ChangeClient
                        (new Client
                        {
                            Id = id,
                            Name = changeDealName.Text,
                            Product = changeDealProduct.Text,
                            Quantity = Convert.ToDouble(changeDealQuantity.Text),
                            Price = Convert.ToDecimal(changeDealPrice.Text),
                            TotalPrice = GetTotalPrice(),
                            CostPrice = GetCostPrice(),
                            Profit = GetProfit(),
                            Note = changeDealNote.Text
                        });
                    products.GetInStock(changeDealProduct.Text, Convert.ToDouble(changeDealQuantity.Text), oldQuantity);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный формат ввода.", "Ошибка!");
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля!", "Ошибка!");
            }
        }

        //Cancel Button
        private void changeClientButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Get a costprice of the product
        decimal GetCostPrice()
        {
            foreach (Product product in products)
            {
                if (product.Name == changeDealProduct.Text)
                {
                    products.Reset();
                    return product.CostPrice * Convert.ToDecimal(changeDealQuantity.Text);
                }
            }
            return 0;
        }

        //
        decimal GetTotalPrice()
        {
            return Convert.ToDecimal(changeDealQuantity.Text) * Convert.ToDecimal(changeDealPrice.Text);
        }

        //Get Profit
        decimal GetProfit()
        {
            return (Convert.ToDecimal(changeDealQuantity.Text) * Convert.ToDecimal(changeDealPrice.Text)) - GetCostPrice(); ;
        }

        //Adding values to changeDealName 
        void AddValuesToNamesBox()
        {
            foreach (Client item in clients)
            {
                if (!changeDealName.Items.Contains(item.Name))
                {
                    changeDealName.Items.Add(item.Name);
                }
            }
        }

        //Adding values to changeDealProduct
        void AddValuesToProductsBox()
        {
            foreach (Product item in products)
            {
                changeDealProduct.Items.Add(item.Name);
            }
        }

        //Adding values to changeDealNote 
        void AddValuesToNotesBox()
        {
            changeDealNote.Items.Add("Оплачен");
            changeDealNote.Items.Add("Не оплачен");
            changeDealNote.Items.Add("Должен ");
        }

        //Calculator
        private void CallCalc(object sender, RoutedEventArgs e)
        {
            StaticValues.CallCalc();
        }

    }
}
