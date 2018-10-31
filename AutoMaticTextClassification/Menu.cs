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
        FileReadWrite frw = new FileReadWrite();
        /// <summary>
        /// Loads the users main menu
        /// </summary>
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
                        SaveBayesingNetwork();
                        menu = true;
                        break;
                    case "2":
                        menu = ChooseABaysingNetwork(frw.GetSavedBayesingNetworks());
                        if (!menu)
                        {
                            SelectText();
                        }
                        string[] result = _bn.GetAnalisedResult().ToArray();
                        foreach (string s in result)
                        {
                            Console.WriteLine(s);
                        }
                        break;
                    case "q":
                        Console.Clear();
                        Console.WriteLine("Exiting Program");
                        Console.ReadKey();
                        menu = false;
                        break;

                    default:
                        menu = true;
                        break;
                }
            } while (menu);

        }
        bool SelectText()
        {
            Console.Clear();
            FileObj[] testData = frw.GetTestData();
            bool ReloadMenu = true;
            string userInput = "";
            int menuOption = 0;
            if (testData.Count() > 0)
            {
                Console.WriteLine("Please Select an Test document to use");
                foreach (FileObj f in testData)
                {
                    menuOption++;
                    Console.WriteLine(menuOption + ". " + f.FileName);
                }
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int result))
                {
                    _bn.GetAnalizedText( testData[result - 1]);
                    ReloadMenu = false;
                }
                else
                {
                    Console.WriteLine("invalid option");
                    Console.WriteLine("Returning to Main Menu");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("there are no tests documents avalible");
                Console.WriteLine("Returning to Main Menu");
                Console.ReadKey();
            }

            return ReloadMenu;
        }
        bool ChooseABaysingNetwork(BayesingNetwork[] bayesingNetworks)
        {
            Console.Clear();
            bool ReloadMenu= true;
            string userInput="";
            int menuOption = 0;
            if (bayesingNetworks.Count()>0)
            {
                Console.WriteLine("Please Select an AI to use");
                foreach (BayesingNetwork bn in bayesingNetworks)
                {
                    menuOption++;
                    Console.WriteLine(menuOption+". "+ bn.Name);
                }
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int result))
                {
                    _bn = bayesingNetworks[result - 1];
                    ReloadMenu = false;
                }
                else
                {
                    Console.WriteLine("invalid option");
                    Console.WriteLine("Returning to Main Menu");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("there are no trained AI avalible");
                Console.WriteLine("Returning to Main Menu");
                Console.ReadKey();
            }

            return ReloadMenu;
        }
        /// <summary>
        /// asks the user if they wish to save their AI
        /// </summary>
        public void SaveBayesingNetwork()
        {
            bool menu = false;  //checks if the menu 
            bool failedFile = false;
            
            do {
                Console.Clear();
                string userInput = "";
                if (failedFile)
                {
                    
                    Console.WriteLine("A folder with that name already exists, unable to save network under that name");
                    Console.WriteLine("Would you like to save the AI under a different name?(y/n)");
                    userInput = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("AI is Trained");
                    Console.WriteLine("Would you like to save the AI (y/n)");
                    userInput = Console.ReadLine();
                }
                Console.Clear();
                failedFile = false;
                menu = false;
                switch (userInput)
                {
                    case "y":
                        do
                        {
                            Console.WriteLine("What do you wish to save the folder as?");
                            userInput = Console.ReadLine();
                        } while (userInput == "");
                        _bn.Name = userInput;
                        failedFile = frw.SaveBayesingToFile(userInput, _bn.KnownInfomation);
                        break;

                    case "n":
                        Console.WriteLine("Are you sure you wish to leave without saving? (y/n)");
                        userInput = Console.ReadLine();
                        menu = (userInput == "y") ? false : true; 
                        break;

                    default:
                        menu = true;
                        break;
                } 
            } while (menu || failedFile);
        }
        /// <summary>
        /// Returns the result of the AI using the test data.
        /// </summary>
        /// <param name="goverment"></param>
        /// <param name="percentageCertainty"></param>
        public void Result(string goverment, double percentageCertainty)
        {

        }
    }
}
