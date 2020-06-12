using DAN_XXXI_Jasmina_Kostadinovic.DBHaendlers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XXXI_Jasmina_Kostadinovic.Menus
{
    class MaunMenu
    {
        public void CreateMenu()
        {
            bool shouldRepeat;
            do
            {
                var dataAccess = new DataAccess();
                shouldRepeat = false;
                Console.WriteLine("---------------------------");
                Console.WriteLine("1. Show all orders\n2. Show single order\n3. Create new order\n4. Update the order\n5. Remove an order\n6. Exit\n");
                string input = Console.ReadLine();
                //Validation of the user input so the app won't crash if the input is wrong
                if (!int.TryParse(input, out int inputNumber) || char.IsWhiteSpace(input[0]))
                {
                    Console.WriteLine("Wrong input! Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                switch (inputNumber)
                {
                    case 1:

                        try
                        {
                           
                        }
                      
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.ToString());
                        }

                        shouldRepeat = true;
                        continue;
                    case 2:

                        try
                        {

                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.ToString());
                        }

                        shouldRepeat = true;
                        continue;
                    case 3:
                        try
                        {

                        }
                     
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.ToString());
                        }

                        shouldRepeat = true;
                        continue;
                    case 4:
                        try
                        {

                        }
                  
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.Message);
                        }

                        shouldRepeat = true;
                        continue;
                    case 5:
                        try
                        {
                          
                        }
                     
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.Message);
                        }
                        shouldRepeat = true;
                        continue;
                    case 6:
                        try
                        {
                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something unexpected happened. Contact the support service for more information...\n");
                            Console.WriteLine(ex.Message);
                        }
                        shouldRepeat = false;
                        continue;
                    default:
                        Console.WriteLine("Wrong input! Please try again.");
                        shouldRepeat = true;
                        break;
                }
            } while (shouldRepeat);
        }
    }
}
