using DAN_XXXI_Jasmina_Kostadinovic.DBHaendlers;
using DAN_XXXI_Jasmina_Kostadinovic.Model;
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
                            List<tblMealOrder> mealOrder = new List<tblMealOrder>();
                            using (var context = new RestaurantEntities())
                            {
                                mealOrder = context.tblMealOrders.Include("tblMeal.tblOrder").ToList();
                            }
                            foreach (var m in mealOrder)
                            {
                                Console.WriteLine("Meal with order number " + m.MealOrderID + " ordered on " + m.tblOrder.DateOfOrder + " to be delivered to adress " + m.tblOrder.AddressOfRecipient + " contains:");
                                foreach (var item in m.tblOrder.tblMealOrders)
                                {
                                    Console.Write(item.tblMeal.Name);
                                    Console.Write("/");
                                }
                            }
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
                            tblMealOrder mealOrder;
                            int orderID;
                            do
                            {
                                Console.WriteLine("Please enter your order ID");
                            } while (int.TryParse(Console.ReadLine(),out orderID) != true);

                            using (var context = new RestaurantEntities())
                            {
                                mealOrder = context.tblMealOrders.Include("tblMeal.tblOrder").Where(m => m.MealOrderID == orderID).FirstOrDefault();
                            }
                            Console.WriteLine("Meal with order number " + mealOrder.MealOrderID + " ordered on " + mealOrder.tblOrder.DateOfOrder + " to be delivered to adress " + mealOrder.tblOrder.AddressOfRecipient + " contains:");
                            foreach (var item in mealOrder.tblOrder.tblMealOrders)
                            {
                                Console.Write(item.tblMeal.Name);
                                Console.Write("/");
                            }
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
                            tblMealOrder mealOrder;
                            int orderID;
                            do
                            {
                                Console.WriteLine("Please enter the ID of the order you wish to delete");
                            } while (int.TryParse(Console.ReadLine(), out orderID));

                            using (var context = new RestaurantEntities())
                            {
                                mealOrder = context.tblMealOrders.Include("tblMeal.tblOrder").Where(m => m.MealOrderID == orderID).FirstOrDefault();
                                try
                                {
                                    context.tblMealOrders.Remove(mealOrder);
                                }
                                catch (SqlException sql)
                                {
                                    Console.WriteLine("Order with that ID doesn't exist in the database");
                                    Console.WriteLine(sql.Message);
                                }
                                
                                Console.WriteLine("Meal Order removed successfully!");
                            }
                            
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
