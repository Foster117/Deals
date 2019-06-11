using System;
using System.Collections;
using System.Collections.Specialized;


namespace Dealer
{
    public class Products : IEnumerable, IEnumerator, INotifyCollectionChanged
    {
        Product[] assortment;
        int cursor;
        Clients clients;
        MainWindow mainWindow;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        //Ctor
        public Products()
        {

        }
        public Products(Clients clients, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            assortment = new Product[0];
            cursor = -1;
            this.clients = clients;
        }

        // Add product
        public void Add(Product item)
        {
            Product[] bufferArray = new Product[assortment.Length + 1];
            assortment.CopyTo(bufferArray, 0);
            bufferArray[assortment.Length] = item;
            assortment = bufferArray;
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            StaticValues.isSaved = false;
            mainWindow.ProductStat();
        }

        // Delete product
        public void Remove(Product product)
        {
               Product[] bufferArray = new Product[assortment.Length - 1];

                int bufferCount = 0;
                for (int i = 0; i < bufferArray.Length; i++)
                {
                    if (assortment[i].ID == product.ID)
                    {
                        bufferCount = 1;
                    }
                    bufferArray[i] = assortment[i + bufferCount];
                    bufferArray[i].ID = i;
                }
                clients.RemoveByProduct(product.Name);
                assortment = bufferArray;

            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            StaticValues.isSaved = false;
            mainWindow.ProductStat();
        }

        // Сheck on the coincidence of names
        public bool CheckName(string name)
        {
            for (int i = 0; i < assortment.Length; i++)
            {
                if (assortment[i].Name.ToLower() == name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //Calculation of number of products in stock
        public void GetInStock(string productName, double productQuantity, double oldQuantity)
        {
            for (int i = 0; i < assortment.Length; i++)
            {
                if (productName == assortment[i].Name)
                {
                    assortment[i].InStock = (assortment[i].InStock + oldQuantity) - productQuantity;
                    if (CollectionChanged != null)
                    {
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                    break;
                }
            }
        }

        //Method of return of number of deleted product
        public void AddRemovedQuantity(string name, double quantity)
        {
            for (int i = 0; i < assortment.Length; i++)
            {
                if (assortment[i].Name == name)
                {
                    assortment[i].InStock += quantity;
                    if (CollectionChanged != null)
                    {
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                    break;
                }
            }
        }

        //Clear the collection
        public void Clear()
        {
            assortment = new Product[0];
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            mainWindow.ProductStat();
        }

        //Interfaces
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this; 
        }
        public int Count { get { return assortment.Length; } }

        public object Current
        {
            get
            {
                return assortment[cursor];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return assortment[cursor];
            }
        }

        public bool MoveNext()
        {
            if (cursor < assortment.Length - 1)
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

        public void Reset()
        {
            cursor = -1;
        }
    }
}

