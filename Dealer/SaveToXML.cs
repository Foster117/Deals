using System;
using System.Windows;
using System.Text;
using System.Xml;

namespace Dealer
{
    class SaveToXML
    {
        Products products;
        Clients clients;
        MainWindow mainWindow;

        
        public SaveToXML(Products products, Clients clients, MainWindow mainWindow)
        {
            this.products = products;
            this.clients = clients;
            this.mainWindow = mainWindow;
        }

        public void Save()
        {
            XmlTextWriter writer = new XmlTextWriter(StaticValues.staticFilename, Encoding.GetEncoding(1251));
            Encryption encryption = new Encryption();
            
            try
            {
                writer.WriteStartDocument();

                //<AllData>
                writer.WriteStartElement("AllData");

                //<Products>
                writer.WriteStartElement("Products");
                //<Product>
                foreach (Product product in products)
                {
                    writer.WriteStartElement("Product");
                    //ID
                    writer.WriteStartElement("Id");
                    writer.WriteValue(product.ID);
                    writer.WriteEndElement();
                    //Name
                    writer.WriteStartElement("Name");
                    writer.WriteValue(product.Name);
                    writer.WriteEndElement();
                    //Quantity
                    writer.WriteStartElement("Quantity");
                    writer.WriteValue(product.Quantity);
                    writer.WriteEndElement();
                    //CostPrice
                    writer.WriteStartElement("CostPrice");
                    writer.WriteValue(product.CostPrice);
                    writer.WriteEndElement();
                    //InStock
                    writer.WriteStartElement("InStock");
                    writer.WriteValue(product.InStock);
                    writer.WriteEndElement();
                    //CostPriceTotal
                    writer.WriteStartElement("CostPriceTotal");
                    writer.WriteValue(product.CostPriceTotal);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                //</Products>
                writer.WriteEndElement();

                //<Clients>
                writer.WriteStartElement("Clients");
                //<Client>
                foreach (Client client in clients)
                {
                    writer.WriteStartElement("Client");

                    //ID
                    writer.WriteStartElement("Id");
                    writer.WriteValue(client.Id);
                    writer.WriteEndElement();
                    //Name
                    writer.WriteStartElement(encryption.StartEncryption("Name"));
                    writer.WriteValue(client.Name);
                    writer.WriteEndElement();
                    //Product
                    writer.WriteStartElement("Product");
                    writer.WriteValue(client.Product);
                    writer.WriteEndElement();
                    //Quantity
                    writer.WriteStartElement("Quantity");
                    writer.WriteValue(client.Quantity);
                    writer.WriteEndElement();
                    //Price
                    writer.WriteStartElement("Price");
                    writer.WriteValue(client.Price);
                    writer.WriteEndElement();
                    //TotalPrice
                    writer.WriteStartElement("TotalPrice");
                    writer.WriteValue(client.TotalPrice);
                    writer.WriteEndElement();
                    //CostPrice
                    writer.WriteStartElement("CostPrice");
                    writer.WriteValue(client.CostPrice);
                    writer.WriteEndElement();
                    //Profit
                    writer.WriteStartElement("Profit");
                    writer.WriteValue(client.Profit);
                    writer.WriteEndElement();
                    //Note
                    writer.WriteStartElement("Note");
                    writer.WriteValue(client.Note);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //<Payments>
                writer.WriteStartElement("Payments");
                //<Payment>
                foreach (string payment in mainWindow.payments)
                {
                    writer.WriteStartElement("Payment");
                    writer.WriteValue(payment);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //Notes
                writer.WriteStartElement("Notes");
                writer.WriteValue(mainWindow.NotesTextBox.Text);
                writer.WriteEndElement();

                writer.Close();
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

