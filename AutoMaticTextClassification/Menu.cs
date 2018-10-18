using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMaticTextClassification
{
    class Menu
    {
        
        public void StartUp()
        {
            string menuOption;
            bool menu = false;
            do
            {
                Console.WriteLine("Select an Option");
                Console.WriteLine("1. Train");
                Console.WriteLine("2. Classify text ");
                Console.WriteLine("q to quit");
                menuOption = Console.ReadLine();
                switch (menuOption)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "q":
                        break;

                    default:
                        menu = true;
                        break;
                }
            } while (menu);

        }
    }
}
