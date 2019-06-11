using System;
using System.Text.RegularExpressions;
using System.Windows;


namespace Dealer
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        Products products;
        const string pattern = @"^[0-9]{1,}($|\.?[0-9]{1,2}$)";

        //Ctors
        public AddProduct()
        {
            InitializeComponent();
        }
        public AddProduct(Products products)
            : this()
        {
            this.products = products;
            newProductName.Focus();
        }

        //Method -- Add Product
        private void ProductAdd()
        {
            products.Add
                (new Product
                {
                    ID = products.Count,
                    Name = newProductName.Text,
                    Quantity = Convert.ToDouble(newProductQuantity.Text),
                    CostPrice = Convert.ToDecimal(newProductCostPrice.Text),
                    InStock = Convert.ToDouble(newProductQuantity.Text),
                    CostPriceTotal = (Convert.ToDecimal(newProductQuantity.Text) * Convert.ToDecimal(newProductCostPrice.Text))
                }               
                );
        }

        //OK Button
        private void awButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (newProductName.Text != "" && newProductQuantity.Text != "" && newProductCostPrice.Text != "")
            {

                if (Regex.IsMatch(newProductQuantity.Text, pattern) && Regex.IsMatch(newProductCostPrice.Text, pattern))
                {
                    if (!products.CheckName(newProductName.Text))
                    {
                        ProductAdd();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Товар с таким именем уже есть.", "Ошибка!");
                    }
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

        //Calculator
        private void CallCalc(object sender, RoutedEventArgs e)
        {
            StaticValues.CallCalc();
        }
    }
}
