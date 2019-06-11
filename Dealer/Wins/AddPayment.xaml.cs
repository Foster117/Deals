using System.Windows;
using System.Text.RegularExpressions;

namespace Dealer
{
    /// <summary>
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class AddPayment : Window
    {
        MainWindow mainWindow;


        public AddPayment()
        {
            InitializeComponent();
        }
        public AddPayment(MainWindow mainWindow)
            : this()
        {
            this.mainWindow = mainWindow;
            addPaymentTextBox.Focus();
        }

        // Button OK
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(addPaymentTextBox.Text, @"^[0-9]{1,}($|\.?[0-9]{1,2}$)"))
            {
                mainWindow.AddPayment(addPaymentTextBox.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный формат ввода!", "Ошибка");
            }
        }

        // Button Cancel
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }


}

