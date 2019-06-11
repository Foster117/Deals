using System.Windows;

namespace Dealer
{
    /// <summary>
    /// Interaction logic for RemoveProductConfirm.xaml
    /// </summary>
    public partial class RemoveProductConfirm : Window
    {
        MainWindow mWindow;
        //Ctors
        public RemoveProductConfirm()
        {
            InitializeComponent();
        }
        public RemoveProductConfirm(MainWindow mWindow)
            : this()
        {
            this.mWindow = mWindow;
        }

        //Yes Button
        private void ConfirmDelProductYesButton_Click(object sender, RoutedEventArgs e)
        {
            mWindow.ProductRemove();
            this.Close();
        }

        //No Button
        private void ConfirmDelProductNoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
