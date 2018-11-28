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
            bool menu = true;
            
            //Ensures the users keeps returning to the menu until they wish to quit.
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
                        //creates a new network
                        _bn = new BayesingNetwork();
                        try
                        {
                            _bn.Train(); 
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("File error, unable to read training data");
                        }
                        SaveBayesingNetwork();
                        menu = true;
                        break;
                    case "2":
                        //gets an already existing network
                        menu = ChooseABaysingNetwork(frw.GetSavedBayesingNetworks());
                        if (!menu)
                        {
                            SelectText();
                            if (_bn != null)
                            {
                                //gets the result of the analysed text
                                string[] result = _bn.GetAnalysedResult().ToArray();
                                Console.Clear();
                                if (result.Count() != 0)
                                {
                                    //displays the result to the user
                                    Console.WriteLine("The results of the analysed text are");
                                    int i = 1;
                                    foreach (string s in result)
                                    {
                                        Console.WriteLine(i + ". " + s);
                                        i++;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No results avalible.");

                                }
                            }
                        }
                        menu = true;
                        break;
                    case "q":
                        //allows the user to escape the program
                        Console.Clear();
                        Console.WriteLine("Exiting Program");
                        menu = false;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option. Please enter 1, 2 or q.");
                        menu = true;
                        break;
                }
                Console.ReadKey();
            } while (menu);
        }
        /// <summary>
        /// Gets the text the user wishes to analise
        /// </summary>
        void SelectText()
        {
            Console.Clear();
            FileObj[] testData = frw.GetTestData();
            string userInput = "";
            int menuOption = 0;
            //checks if there is any test data and displays it
            if (testData.Count() > 0)
            {
                //takes the users choice
                Console.WriteLine("Please Select an Test document to use");
                foreach (FileObj f in testData)
                {
                    menuOption++;
                    Console.WriteLine(menuOption + ". " + f.FileName);
                }
                userInput = Console.ReadLine();
                //checks if the user input is valid and analizes it if it is
                if (int.TryParse(userInput, out int result))
                {
                    _bn.GetAnalysedText( testData[result - 1]);
                }
                else
                {
                    Console.WriteLine("Invalid option");
                    Console.WriteLine("Returning to Main Menu");
                    Console.ReadKey();
                }            
            }
            // tells the user there is no test data
            else
            {
                Console.WriteLine("There are no test documents available");
                Console.WriteLine("Returning to Main Menu");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// displays a list of posible networks for the user to choose from
        /// </summary>
        /// <param name="bayesingNetworks">A list of avalible networks</param>
        /// <returns>whether the menu needs to be reloaded</returns>
        bool ChooseABaysingNetwork(BayesingNetwork[] bayesingNetworks)
        {
            Console.Clear();
            bool reloadMenu= true; //used to determine if the network needs reloaded
            string userInput=""; //basic userinput
            int menuOption = 0; //keeps track of the amount of options

           //checks if it can display any networks
            if (bayesingNetworks.Count() > 0)
            {
                //lists all the available networks
                Console.WriteLine("Please select an AI to use");
                foreach (BayesingNetwork bn in bayesingNetworks)
                {
                    menuOption++;
                    Console.WriteLine(menuOption + ". " + bn.Name);
                }
                userInput = Console.ReadLine();
                //checks if the user selected a valid network
                if (int.TryParse(userInput, out int result)&& result <= bayesingNetworks.Count())
                {
                    _bn = bayesingNetworks[result - 1];
                    reloadMenu = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    Console.WriteLine("Returning to Main Menu");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("There are no trained AI available");
                Console.WriteLine("Returning to Main Menu");
                Console.ReadKey();
            }
            return reloadMenu;
        }
        /// <summary>
        /// asks the user if they wish to save their AI
        /// </summary>
        void SaveBayesingNetwork()
        {
            
            bool menu = false; //determins whether the mune needds reloaded
            bool fileAlreadyExists = false; //used to determine if the file can be created under the users input

            do {
                Console.Clear();
                string userInput = "";
                if (fileAlreadyExists)
                {
                    //informs the user the name is taken, asks them if they wish to rename
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
                fileAlreadyExists = false;
                menu = false;
                switch (userInput.ToLower())
                {
                    case "y":
                        do
                        {
                            //asks the user to name the file
                            Console.WriteLine("What do you wish to save the folder as?");
                            userInput = Console.ReadLine();
                        } while (userInput == "");
                        _bn.Name = userInput;
                        //checks if the file exists
                        fileAlreadyExists = frw.SaveBayesingToFile(userInput, _bn.KnownInfomation);
                        break;
                    case "n":
                        //checks if the user is sure they don't wish to save the new file
                        Console.WriteLine("Are you sure you wish to leave without saving? (y/n)");
                        userInput = Console.ReadLine();
                        menu = (userInput == "y") ? false : true; 
                        break;
                    default:
                        menu = true;
                        break;
                } 
            } while (menu || fileAlreadyExists);
        }
    }
}
