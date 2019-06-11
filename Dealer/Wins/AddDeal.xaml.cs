using System;
using System.Text.RegularExpressions;
using System.Windows;


namespace Dealer
{
    /// <summary>
    /// Interaction logic for AddDeal.xaml
    /// </summary>
    public partial class AddDeal : Window
    {
        Products products;
        Clients clients;
        const string pattern = @"^[0-9]{1,}($|\.?[0-9]{1,2}$)";

        //Ctors
        public AddDeal()
        {
            InitializeComponent();
        }
        public AddDeal(Products products, Clients clients)
            :this()
        {
            InitializeComponent();
            this.products = products;
            this.clients = clients;
            newDealName.Focus();

            //Adding values to ComboBoxes
            AddValuesToNamesCBox();
            AddValuesToProductsCBox();
            AddValuesToNotesCBox();
        }



        // OK Button
        private void awButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (newDealName.Text != "" && newDealProduct.Text != "" && newDealNote.Text != "" && newDealPrice.Text != "" && newDealQuantity.Text != "")
            {
                if (Regex.IsMatch(newDealQuantity.Text, pattern) && Regex.IsMatch(newDealPrice.Text, pattern))
                {
                    ClientAdd();
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
        private void awButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Add client
        private void ClientAdd()
        {
            clients.Add
            (new Client
            {
                Id = clients.Count,
                Name = newDealName.Text,
                Product = newDealProduct.Text,
                Quantity = Convert.ToDouble(newDealQuantity.Text),
                Price = Convert.ToDecimal(newDealPrice.Text),
                TotalPrice = GetTotalPrice(),
                CostPrice = GetCostPrice(),
                Profit = GetProfit(),
                Note = newDealNote.Text
            });
            products.GetInStock(newDealProduct.Text, Convert.ToDouble(newDealQuantity.Text), 0);
        }

        //Get a costprice of the product
        decimal GetCostPrice()
        {
            foreach (Product product in products)
            {
                if (product.Name == newDealProduct.Text)
                {
                    products.Reset();
                    return product.CostPrice * Convert.ToDecimal(newDealQuantity.Text);
                }
            }
            return 0;
        }

        //
        decimal GetTotalPrice()
        {
            return Convert.ToDecimal(newDealQuantity.Text) * Convert.ToDecimal(newDealPrice.Text);
        }

        //Get profit
        decimal GetProfit()
        {
            return (Convert.ToDecimal(newDealQuantity.Text) * Convert.ToDecimal(newDealPrice.Text)) - GetCostPrice(); ;
        }

        //Adding values to newDealName 
        void AddValuesToNamesCBox()
        {
            foreach (Client item in clients)
            {
                if (!newDealName.Items.Contains(item.Name))
                {
                    newDealName.Items.Add(item.Name);
                }

            }
        }

        //Adding values to newDealProduct 
        void AddValuesToProductsCBox()
        {
            if (products.Count == 1)
            {
                products.MoveNext();
                newDealProduct.Items.Add(((Product)products.Current).Name);
                newDealProduct.SelectedItem = ((Product)products.Current).Name;
                products.Reset();
            }
            else
            {
                foreach (Product item in products)
                {
                    newDealProduct.Items.Add(item.Name);
                }
            }
        }

        //Quantity UP
        private void quantityUp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (newDealProduct.SelectedValue != null)
            {
                foreach (Product item in products)
                {
                    if (item.Name == newDealProduct.SelectedValue.ToString())
                    {
                        newDealQuantity.Text = item.InStock.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать товар!");
            }
        }

        //Adding values to newDealNote 
        void AddValuesToNotesCBox()
        {
            newDealNote.Items.Add("Оплачен");
            newDealNote.Items.Add("Не оплачен");
            newDealNote.Items.Add("Должен ");
        }

        //Calculator
        private void CallCalc(object sender, RoutedEventArgs e)
        {
            StaticValues.CallCalc();
        }

        private void Client_Add_IButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }


    }
}
