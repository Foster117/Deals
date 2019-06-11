using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using Dealer.Wins;
using System.Windows.Input;

namespace Dealer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static string staticFilename = null;
        //public static bool isSaved = true;
        //public const string title = "Deals 1.1";

        public ArrayList payments = new ArrayList();
        public Products products;
        public Clients clients;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            if (SystemParameters.WorkArea.Height <= 800)
            {
                this.Height = SystemParameters.WorkArea.Height - 100;
            }

            //Collections
            clients = new Clients(products, this);
            products = new Products(clients, this);

            //DataGrids
            productsGrid.ItemsSource = products;
            clientsGrid.ItemsSource = clients;

            if (args.Length > 0)
            {
                string filePath = args[0];
                if (File.Exists(filePath))
                {
                    StaticValues.staticFilename = filePath.ToString();
                    OpenFile(filePath);
                    dealerWindow.Title = StaticValues.title + " | " + StaticValues.staticFilename;
                }
            }
            else
            {
                dealerWindow.Title = StaticValues.title;
            }
            ProductStat();
        }



        //--------------------PRODUCTS--------------------------
        //Button -- Add Product (Get Window)
        private void Product_Add_IButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddProduct addProduct = new AddProduct(products);
            addProduct.ShowDialog();
        }

        //Button -- Remove Product (Get Confirmation Window)
        private void Product_Remove_IButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (productsGrid.SelectedItem != null)
            {
                RemoveProductConfirm removeProductConfirm = new RemoveProductConfirm(this);
                removeProductConfirm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.");
            }
        }

        //Method -- Remove Product
        public void ProductRemove()
        {
            products.Remove(productsGrid.SelectedItem as Product);
        }
        //------------------------------------------------------


        //--------------------CLIENTS---------------------------
        //Button -- Add Client (Get Window)
        private void Client_Add_IButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (products.Count != 0)
            {
                AddDeal addDeal = new AddDeal(products, clients);
                addDeal.ShowDialog();
            }
            else
            {
                MessageBox.Show("Для добавления покупателя необходимо наличие товара.");
            }
        }

        //Button -- Remove Client
        private void Client_Remove_IButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveClientM();
        }

        //Button -- Edit Client (Get Window)
        private void image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (clientsGrid.SelectedItem != null)
            {
                ChangeClient changeClient = new ChangeClient(clients, products, (Client)clientsGrid.SelectedItem);
                changeClient.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сделку для редактирования.");
            }
        }

        //Method -- Remove Client
        public void RemoveClientM()
        {
            if (clients.Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранного клиента?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    products.AddRemovedQuantity(((Client)clientsGrid.SelectedItem).Product, ((Client)clientsGrid.SelectedItem).Quantity);
                    clients.Remove((clientsGrid.SelectedItem as Client).Id);
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать клиента из списка.");
            }
        }
        //------------------------------------------------------


        //---------------------PAYMENTS-------------------------
        //Button -- AddPayment
        private void paymentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (products.Count != 0)
            {
                AddPayment addPayment = new AddPayment(this);
                addPayment.ShowDialog();
            }
            else
            {
                MessageBox.Show("Невозможно добавить платеж, т.к. в списке нет ни одного товара.", "Ошибка");
            }
        }

        //Button -- RemovePayment
        private void paymentRemmoveButton_Click(object sender, RoutedEventArgs e)
        {
            payments.Remove(paymentsListBox.SelectedItem);
            paymentsListBox.Items.Refresh();
            Calculate();
        }

        //Method -- Add Payment
        public void AddPayment(string payment)
        {
            payments.Add(payment);
            paymentsListBox.ItemsSource = payments;
            paymentsListBox.Items.Refresh();
            Calculate();
            StaticValues.isSaved = false;
        }

        //Method -- Calculate
        private void Calculate()
        {
            decimal sum = 0;
            if (products.Count != 0)
            {
                for (int i = 0; i < payments.Count; i++)
                {
                    sum += Convert.ToDecimal(payments[i]);
                }
                paymentsRestValueLabel.Content = (Convert.ToDecimal(productsTotalCostPriceLabel.Content) - sum).ToString();
            }
            else
            {
                paymentsRestValueLabel.Content = "0";
            }
        }

        //Method -- Refresh
        public void Refresh()
        {
            paymentsListBox.ItemsSource = payments;
            paymentsListBox.Items.Refresh();
            Calculate();
        }
        //------------------------------------------------------


        //----------------------STAT----------------------------
        public void ProductStat()
        {
            double totalQuantity = 0;
            decimal totalPrice = 0;
            double totalSold = 0;
            decimal totalProfit = 0;

            foreach (Product product in products)
            {
                totalQuantity += product.Quantity;
                totalPrice += product.CostPriceTotal;
            }
            foreach (Client client in clients)
            {
                totalSold += client.Quantity;
                totalProfit += client.Profit;
            }
            productsTotalQuantityLabel.Content = totalQuantity.ToString();
            productsTotalCostPriceLabel.Content = totalPrice.ToString();
            productsSoldTotal.Content = totalSold;
            productsProfitTotal.Content = totalProfit;
            Calculate();
        }
        //------------------------------------------------------


        //-----------------------MENU---------------------------
        //NewDocument
        private void NewDocument_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            products.Clear();
            clients.Clear();
            payments.Clear();
            paymentsListBox.Items.Refresh();
            StaticValues.staticFilename = null;
            StaticValues.isSaved = true;
            dealerWindow.Title = StaticValues.title;
            NotesTextBox.Text = null;
        }

        //Save
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveToExistingFile();
        }

        //SaveAs
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveByDialog();
        }

        //Open
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            products.Clear();
            clients.Clear();
            StaticValues.staticFilename = null;
            StaticValues.isSaved = true;
            OpenFile();
        }

        //Exit
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Add Product
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct(products);
            addProduct.ShowDialog();
        }

        //Remove Product
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (productsGrid.SelectedItem != null)
            {
                RemoveProductConfirm removeProductConfirm = new RemoveProductConfirm(this);
                removeProductConfirm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.");
            }
        }

        //Add Client
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if (products.Count != 0)
            {
                AddDeal addDeal = new AddDeal(products, clients);
                addDeal.ShowDialog();
            }
            else
            {
                MessageBox.Show("Для добавления покупателя необходимо наличие товара.");
            }
        }

        //Remove Client
        private void RemoveClient_Click(object sender, RoutedEventArgs e)
        {
            RemoveClientM();
        }

        //Edit Client
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientsGrid.SelectedItem != null)
            {
                ChangeClient changeClient = new ChangeClient(clients, products, (Client)clientsGrid.SelectedItem);
                changeClient.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сделку для редактирования.");
            }
        }

        //About
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        //Calculator
        private void CallCalc(object sender, RoutedEventArgs e)
        {
            StaticValues.CallCalc();
        }

        //------------------------------------------------------


        //-----------------------SAVE & OPEN--------------------
        //Save by FileDialog
        private void SaveByDialog()
        {
            SaveFileDialog saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "dls файл | *.dls";
            if (saveAsDialog.ShowDialog() == true)
            {
                SaveToXML save = new SaveToXML(products, clients, this);
                StaticValues.staticFilename = saveAsDialog.FileName;
                save.Save();
                StaticValues.isSaved = true;
            }
        }

        //Save to existing file
        private void SaveToExistingFile()
        {
            if (StaticValues.staticFilename != null)
            {
                SaveToXML save = new SaveToXML(products, clients, this);
                save.Save();
                StaticValues.isSaved = true;
            }
            else
            {
                SaveByDialog();
            }
        }

        //Open file
        private void OpenFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "dls файл | *.dls";
            if (openDialog.ShowDialog() == true)
            {
                StaticValues.staticFilename = openDialog.FileName;
                OpenXML open = new OpenXML(products, clients, this);
                open.Load();
            }
        }

        private void OpenFile(string path)
        {
            StaticValues.staticFilename = path;
            OpenXML open = new OpenXML(products, clients, this);
            open.Load();
        }

        //Save when the process is going to be closed
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            string message;
            if (!StaticValues.isSaved)
            {
                if (StaticValues.staticFilename == null)
                {
                    message = "Вы хотите сохранить изменения?";
                }
                else
                {
                    message = "Вы хотите сохранить изменения в файле " + StaticValues.staticFilename + "?";
                }

                MessageBoxResult result = MessageBox.Show(message, "Имеются несохранённые данные", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveToExistingFile();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        //Do you want to save changes?
        public void SaveChanges()
        {
            if (StaticValues.isSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения?", "Имеются несохранённые данные", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveToExistingFile();
                }
            }
        }

        private void NotesTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            StaticValues.isSaved = false;
        }


        // Hide columns with cost price information
        private void HideColumns(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (productsGrid.Columns[2].Visibility == Visibility.Visible)
            {
                productsGrid.Columns[2].Visibility = Visibility.Hidden;
                productsGrid.Columns[4].Visibility = Visibility.Hidden;
                clientsGrid.Columns[3].Visibility = Visibility.Hidden;
                clientsGrid.Columns[4].Visibility = Visibility.Hidden;
                clientsGrid.Columns[5].Visibility = Visibility.Hidden;
                clientsGrid.Columns[6].Visibility = Visibility.Hidden;
                InfoGroupBox.Visibility = Visibility.Hidden;
                PaymentsGroupBox.Visibility = Visibility.Hidden;
                Row4.Height = new GridLength(0);
            }
            else
            {
                productsGrid.Columns[2].Visibility = Visibility.Visible;
                productsGrid.Columns[4].Visibility = Visibility.Visible;
                clientsGrid.Columns[3].Visibility = Visibility.Visible;
                clientsGrid.Columns[4].Visibility = Visibility.Visible;
                clientsGrid.Columns[5].Visibility = Visibility.Visible;
                clientsGrid.Columns[6].Visibility = Visibility.Visible;
                InfoGroupBox.Visibility = Visibility.Visible;
                PaymentsGroupBox.Visibility = Visibility.Visible;
                Row4.Height = new GridLength(201);
            }
        }
        //-------------------------------------------------------
    }
}