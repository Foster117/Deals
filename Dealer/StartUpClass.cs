using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dealer
{
    class StartUpClass
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            MainWindow window = new MainWindow(args);
            app.Run(window);
        }
    }
}
