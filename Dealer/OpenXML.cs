using System;
using System.Collections;
using System.Windows;
using System.Xml;

namespace Dealer
{
    class OpenXML
    {
        Products products;
        Clients clients;
        MainWindow mainWindow;
        string[] itemsArray;

        
        public OpenXML(Products products, Clients clients, MainWindow mainWindow)
        {
            this.products = products;
            this.clients = clients;
            this.mainWindow = mainWindow;
        }

        public void Load()
        {
            try
            {
                products.Clear();
                clients.Clear();
                mainWindow.NotesTextBox.Text = null;
                Decryption decryption = new Decryption();
                XmlDocument doc = new XmlDocument();
                doc.Load(StaticValues.staticFilename);
                XmlNode root = doc.DocumentElement;

                foreach (XmlNode mainTag in root.ChildNodes)
                {
                    if (mainTag.Name == "Products")
                    {
                        foreach (XmlNode product in mainTag.ChildNodes)
                        {
                            itemsArray = new string[6];
                            int arrayCount = 0;
                            foreach (XmlNode item in product)
                            {
                                itemsArray[arrayCount] = item.InnerText;
                                arrayCount++;
                            }
                            products.Add
                                (new Product
                                {
                                    ID = Convert.ToInt32(itemsArray[0]),
                                    Name = itemsArray[1],
                                    Quantity = Convert.ToDouble(itemsArray[2]),
                                    CostPrice = Convert.ToDecimal(itemsArray[3]),
                                    InStock = Convert.ToDouble(itemsArray[4]),
                                    CostPriceTotal = Convert.ToDecimal(itemsArray[5])
                                }
                                );
                        }
                    }
                    if (mainTag.Name == "Clients")
                    {
                        foreach (XmlNode client in mainTag.ChildNodes)
                        {
                            itemsArray = new string[9];
                            int arrayCount = 0;
                            foreach (XmlNode item in client)
                            {
                                itemsArray[arrayCount] = item.InnerText;
                                arrayCount++;
                            }
                            clients.Add(new Client
                            {
                                Id = Convert.ToInt32(itemsArray[0]),
                                Name = itemsArray[1],
                                Product = itemsArray[2],
                                Quantity = Convert.ToDouble(itemsArray[3]),
                                Price = Convert.ToDecimal(itemsArray[4]),
                                TotalPrice = Convert.ToDecimal(itemsArray[5]),
                                CostPrice = Convert.ToDecimal(itemsArray[6]),
                                Profit = Convert.ToDecimal(itemsArray[7]),
                                Note = itemsArray[8]
                            });
                        }
                    }
                    if (mainTag.Name == "Payments")
                    {
                        ArrayList bufferPayments = new ArrayList();
                        foreach (XmlNode payment in mainTag.ChildNodes)
                        {
                            bufferPayments.Add(payment.InnerText);
                        }
                        mainWindow.payments = bufferPayments;
                        mainWindow.Refresh();
                    }
                    if (mainTag.Name == "Notes" && mainTag.HasChildNodes)
                    {
                        mainWindow.NotesTextBox.Text = mainTag.FirstChild.Value;
                    }
                }

                StaticValues.isSaved = true;
                mainWindow.dealerWindow.Title = StaticValues.title + " | " + StaticValues.staticFilename;
            }

            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
         }
    }
}

