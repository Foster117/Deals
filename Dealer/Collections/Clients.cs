using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealer
{
    public class Clients : IEnumerable, IEnumerator, INotifyCollectionChanged
    {
        Client[] clients;
        Products products;
        MainWindow mainWindow;
        int cursor;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        //Ctor
        public Clients(Products products, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            clients = new Client[0];
            cursor = -1;
            this.products = products;
        }

        //Returns an array length to add a new client
        public int Count
        {
            get { return clients.Length; }
        }

        //Adding a client
        public void Add(Client client)
        {
            Client[] bufferArray = new Client[clients.Length + 1];
            clients.CopyTo(bufferArray, 0);
            bufferArray[clients.Length] = client;
            clients = bufferArray;
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            StaticValues.isSaved = false;
            mainWindow.ProductStat();
        }

        //Deleting a client
        public void Remove(int id)
        {
            if (CheckID(id))
            {
                Client[] bufferArray = new Client[clients.Length - 1];

                int bufferCount = 0;
                for (int i = 0; i < bufferArray.Length; i++)
                {
                    if (clients[i].Id == id)
                    {
                        bufferCount = 1;
                    }
                    bufferArray[i] = clients[i + bufferCount];
                    bufferArray[i].Id = i;
                }
                clients = bufferArray;
                if (CollectionChanged != null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                StaticValues.isSaved = false;
                mainWindow.ProductStat();
            }
        }

        //Deleting clients who bought a product
        public void RemoveByProduct(string productToRemove)
        {
            for (int i = clients.Length - 1; i >= 0; i--)
            {
                if(clients[i].Product.ToLower() == productToRemove.ToLower())
                {
                    Remove(i);
                }
            }
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        //Compare ID
        public bool CheckID(int id)
        {
            if (id >= 0 && id < clients.Length)
            {
                return true;
            }
            return false;
        }

        //Change a client's data
        public void ChangeClient(Client changedClient)
        {
            clients[changedClient.Id] = changedClient;
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            StaticValues.isSaved = false;
            mainWindow.ProductStat();
        }

        //Clear the collection
        public void Clear()
        {
            clients = new Client[0];
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            StaticValues.isSaved = false;
            mainWindow.ProductStat();
        }


        //Interfaces
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (cursor < clients.Length - 1)
            {
                cursor++;
                return true;
            }
            else
            {
                Reset();
                return false;
            }
        }

        public object Current
        {
            get
            {
                return clients[cursor];
            }
        }

        public void Reset()
        {
            cursor = -1;
        }
    }
}

