using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dealer
{
    static class StaticValues
    {
        public static string staticFilename = null;
        public static bool isSaved = true;
        public const string title = "Deals 1.1";

        //Calculator
        public static void CallCalc()
        {
            System.Diagnostics.Process.Start("calc.exe");
        }
    }
}
