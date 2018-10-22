using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class Menu
    {
        BayesingNetwork _bn;
        public void StartUp()
        {
            string menuOption;
            bool menu = false;
            
            do
            {
                Console.Clear();
                Console.WriteLine("Select an Option");
                Console.WriteLine("1. Train");
                Console.WriteLine("2. Classify text ");
                Console.WriteLine("q to quit");
                menuOption = Console.ReadLine();
                switch (menuOption)
                {
                    case "1":
                        _bn = new BayesingNetwork();
                        _bn.Train();
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

        public void SaveBayesingNetwork()
        {
            bool menu = false;
            bool failedFile = false;
            string userInput = "";
            Console.Clear();
            Console.WriteLine("AI is Trained");
            Console.WriteLine("Would you like to save the AI (y/n)");
            userInput = Console.ReadLine();
            FileReadWrite frw = new FileReadWrite();
            do {
                if (failedFile)
                {
                    Console.Clear();
                    Console.WriteLine("A folder with that name already exists, unable to save network under that name");
                    Console.WriteLine("Would you like to save the AI under a different name?(y/n)");
                    userInput = Console.ReadLine();
                }
                switch (userInput)
                {
                    case "y":
                        do
                        {
                            Console.WriteLine("What do you wish to save the folder as?");
                            userInput = Console.ReadLine();
                        } while (userInput != "");
                        failedFile = frw.SaveBayesingToFile(userInput, _bn.KnownInfomation);
                        break;

                    case "n":
                        break;

                    default:
                        menu = true;
                        break;
                } 
            } while (menu || failedFile);
        }


        public void Result(string goverment, double percentageCertainty)
        {

        }
    }
}
