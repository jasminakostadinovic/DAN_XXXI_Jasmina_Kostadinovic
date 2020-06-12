using DAN_XXXI_Jasmina_Kostadinovic.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XXXI_Jasmina_Kostadinovic
{
    class Program
    {
        static void Main(string[] args)
        {
            MaunMenu mm = new MaunMenu();
            mm.CreateMenu();
            Console.ReadLine();
        }
    }
}
