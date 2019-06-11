using System.Text.RegularExpressions;
using System.Windows;

namespace Dealer
{
    /// <summary>
    /// !!!ЭЛЕМЕНТ НЕ АКТУАЛЕН И ПОДЛЕЖИТ УДАЛЕНИЮ!!!
    /// </summary>
    public partial class ChangeProduct : Window
    {
        const string pattern = @"^[0-9]{1,}($|\.?[0-9]{1,2}$)";
        Products products;
        Clients clients;
        Product productToChange;
        string oldName;

        //Конструкторы
        public ChangeProduct()
        {
            InitializeComponent();
        }
        public ChangeProduct(Products products, Clients clients, Product product)
            : this()
        {
            this.products = products;
            this.clients = clients;
            productToChange = product;
            oldName = product.Name;
            changeProductName.Text = productToChange.Name;
            changeProductQuantity.Text = productToChange.Quantity.ToString();
            changeProductCostPrice.Text = productToChange.CostPrice.ToString();
        }

        //Кнопка ОК
        private void changeProductOK_Click(object sender, RoutedEventArgs e)
        {
            if (changeProductName.Text != "" && changeProductQuantity.Text != "" && changeProductCostPrice.Text != "")
            {

                if (Regex.IsMatch(changeProductQuantity.Text, pattern) && Regex.IsMatch(changeProductCostPrice.Text, pattern))
                {
                    if (changeProductName.Text != productToChange.Name)
                    {
                        if (!products.CheckName(changeProductName.Text))
                        {
                            //products.Change(productToChange.ID, changeProductName.Text, Convert.ToDouble(changeProductQuantity.Text), Convert.ToDecimal(changeProductCostPrice.Text));
                            //clients.ChangeProductName(oldName, changeProductName.Text);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Товар с таким именем уже есть.", "Ошибка!");
                        }
                    }
                    else
                    {
                        //products.Change(productToChange.ID, changeProductName.Text, Convert.ToDouble(changeProductQuantity.Text), Convert.ToDecimal(changeProductCostPrice.Text));
                        this.Close();
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

        //Кнопка Отмена
        private void changeProductCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
