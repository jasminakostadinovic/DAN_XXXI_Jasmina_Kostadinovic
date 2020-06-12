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
                            var db = new DataAccess();
                            var orders = db.LoadOrders();

                            if (!orders.Any())
                            {
                                Console.WriteLine("There are no available orders to be shown.");
                                shouldRepeat = true;
                                continue;
                            }
                            var mealsWithOrders = db.LoadOrdersAndMeals();
                            foreach (var order in orders)
                            {
                                Console.WriteLine($"Order id: {order.OrderID}, address: {order.AddressOfRecipient}, price: {order.Price}");
                                if (mealsWithOrders.Any())
                                {
                                    var mealsIds = mealsWithOrders.Where(m => m.OrderID == order.OrderID).ToList();
                                    var meals = new List<tblMeal>();
                                    foreach (var meal in mealsIds)
                                        meals.Add(db.LoadMeal(meal.MealID));

                                    Console.WriteLine("Your order include:");
                                    var no = 1;
                                    foreach (var meal in meals)
                                    {
                                        Console.WriteLine($"{no}. {meal.Name}");
                                        no++;
                                    }
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
                            int orderID;
                            do
                            {
                                Console.WriteLine("Please enter your order ID");
                            } while (int.TryParse(Console.ReadLine(),out orderID) != true);

                            var db = new DataAccess();

                            var order = db.LoadOrder(orderID);
                            var mealsWithOrders = db.LoadOrdersAndMeals();                       
                     

                            if(order != null)
                            {
                                Console.WriteLine($"Order id: {order.OrderID}, address: {order.AddressOfRecipient}, price: {order.Price}");
                                if (mealsWithOrders.Any())
                                {
                                    var mealsIds = mealsWithOrders.Where(m => m.OrderID == orderID).ToList();
                                    var meals = new List<tblMeal>();
                                    foreach (var meal in mealsIds)
                                        meals.Add(db.LoadMeal(meal.MealID));

                                    Console.WriteLine("Your order include:");
                                    var no = 1;
                                    foreach (var meal in meals)
                                    {
                                        Console.WriteLine($"{no}. {meal.Name}");
                                        no++;
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine($"There is no order with id {orderID}");
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
                            var newOrder = new tblOrder();
                            var db = new DataAccess();
                            var meals = db.LoadMeals();

                            if (!meals.Any())
                            {
                                Console.WriteLine("Unfortunately, we are unable to receive any order at the moment.");
                                shouldRepeat = true;
                                continue;
                            }
                            bool stopOrdering = false;
                            var orderedMeals = new Dictionary<tblMeal, int>();
                            do
                            {
                                stopOrdering = false;
                                Console.WriteLine("Please, enter the serial number of the meal you would like to order:");
                                int serialNo = 1;
                                foreach (var item in meals)
                                {
                                    Console.WriteLine($"{serialNo}. {item.Name} - {item.Price} eur");
                                    serialNo++;
                                }
                                var inputForMealNo = GetMealNo(serialNo);
                                if (inputForMealNo == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                var mealIndex = int.Parse(inputForMealNo) - 1;
                                var meal = meals[mealIndex];
                                Console.WriteLine($"Enter the amount of {meal.Name} portions");

                                var numberOfMeal = GetNumberOfMeal(meal.Name);
                                if (numberOfMeal == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                orderedMeals.Add(meal, int.Parse(numberOfMeal));
                                Console.WriteLine("Would you like to order more meals? (yes/no)");
                                var moreMeals = CheckToContinue();
                                if (numberOfMeal == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                if(moreMeals == "yes")
                                {
                                    stopOrdering = false;
                                    meals.Remove(meal);
                                    if (!meals.Any())
                                    {
                                        stopOrdering = true;
                                        Console.WriteLine("There are no more meals on the menu.");
                                    }
                                    continue;
                                }
                                stopOrdering = true;
                            }
                            while (!stopOrdering);

                            Console.WriteLine("Enter the address for delivering:");
                            var addressInput = GetAddress();

                            if (addressInput == "#")
                            {
                                shouldRepeat = true;
                                continue;
                            }
                            newOrder.AddressOfRecipient = addressInput;
                            newOrder.DateOfOrder = DateTime.Now;
                            
                            newOrder.Price = CalculatePrice(orderedMeals);

                            db.AddNewOrder(newOrder);
                            var orders = db.LoadOrders();
                            var idOrder = orders.Last().OrderID;

                            foreach(var meal in orderedMeals)
                            {
                                var mealOrder = new tblMealOrder();
                                mealOrder.OrderID = idOrder;
                                mealOrder.MealID = meal.Key.MealID;
                                db.AddNewOrderedMeal(mealOrder);
                            }

                            Console.WriteLine($"You have successfully created new order with id {idOrder}");
                            Console.WriteLine("Your order include:");
                            var no = 1;
                            foreach (var meal in orderedMeals)
                            {
                                Console.WriteLine($"{no}. {meal.Key.Name} - {meal.Value} portions");
                                no++;
                            }
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
                            DataAccess da = new DataAccess();
                            bool success = false;
                            int ID;
                            do
                            {
                                Console.WriteLine("ID of order you want to update");
                                success = Int32.TryParse(Console.ReadLine(), out ID);

                                if (!success)
                                {
                                    Console.WriteLine("invalid input");
                                }
                            } while (!success);

                            tblOrder updateOrder = da.LoadOrder(ID);

                            if (updateOrder == null)
                            {
                                Console.WriteLine("There is no order with this ID");
                                shouldRepeat = true;
                                continue;
                            }

                          List <tblMeal> meals = da.LoadMeals();
                                                     
                            bool stopOrdering = false;
                            var orderedMeals = new Dictionary<tblMeal, int>();
                            do
                            {
                                stopOrdering = false;
                                Console.WriteLine("Please, enter the serial number of the meal you would like to order:");
                                int serialNo = 1;
                                foreach (var item in meals)
                                {
                                    Console.WriteLine($"{serialNo}. {item.Name} - {item.Price} eur");
                                    serialNo++;
                                }
                                var inputForMealNo = GetMealNo(serialNo);
                                if (inputForMealNo == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                var mealIndex = int.Parse(inputForMealNo) - 1;
                                var meal = meals[mealIndex];
                                Console.WriteLine($"Enter the amount of {meal.Name} portions");

                                var numberOfMeal = GetNumberOfMeal(meal.Name);
                                if (numberOfMeal == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                orderedMeals.Add(meal, int.Parse(numberOfMeal));
                                Console.WriteLine("Would you like to order more meals? (yes/no)");
                                var moreMeals = CheckToContinue();
                                if (numberOfMeal == "#")
                                {
                                    shouldRepeat = true;
                                    stopOrdering = true;
                                    continue;
                                }
                                if (moreMeals == "yes")
                                {
                                    stopOrdering = false;
                                    meals.Remove(meal);
                                    if (!meals.Any())
                                    {
                                        stopOrdering = true;
                                        Console.WriteLine("There are no more meals on the menu.");
                                    }
                                    continue;
                                }
                                stopOrdering = true;
                            }
                            while (!stopOrdering);

                            updateOrder.Price = CalculatePrice(orderedMeals);

                            da.UpdateOrder(updateOrder);

                            var oldOrderedMeals = da.LoadOrdersAndMeals().Where(o => o.OrderID == updateOrder.OrderID).ToList();

                            foreach(var o in oldOrderedMeals)
                            {
                                da.RemoveOrderedMeal(o.OrderID);
                            }
                            foreach (var meal in orderedMeals)
                            {
                                var mealOrder = new tblMealOrder();
                                mealOrder.OrderID = updateOrder.OrderID;
                                mealOrder.MealID = meal.Key.MealID;
                                da.AddNewOrderedMeal(mealOrder);
                            }

                            Console.WriteLine($"You have successfully updated the order with id {updateOrder.OrderID}");
                            Console.WriteLine("Your order now include:");
                            var no = 1;
                            foreach (var meal in orderedMeals)
                            {
                                Console.WriteLine($"{no}. {meal.Key.Name} - {meal.Value} portions");
                                no++;
                            }

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
                            int orderID;
                            do
                            {
                                Console.WriteLine("Please enter the ID of the order you wish to delete");
                            } while (int.TryParse(Console.ReadLine(), out orderID) != true);


                            var db = new DataAccess();
                            var orderToRemove = db.LoadOrder(orderID);

                            if(orderToRemove == null)
                            {
                                Console.WriteLine($"Order with ID '{orderID}' doesn't exist in the database");
                            }
                            else
                            {
                                db.RemoveOrder(orderID);
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

        private string GetAddress()
        {
            string consoleInput = string.Empty;
            bool shouldRepeat;
            do
            {
                shouldRepeat = false;
                Console.WriteLine(">>>To get back to the menu press '#' + ENTER<<<");
                consoleInput = Console.ReadLine();
                //if the customer has chosen to abort the action
                if (consoleInput == "#")
                    continue;

                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    Console.WriteLine($"Wrong input! Plese try again.");
                    shouldRepeat = true;
                    continue;
                }
                shouldRepeat = false;
            } while (shouldRepeat);
            return consoleInput;
        }

        private string CheckToContinue()
        {
            string consoleInput = string.Empty;
            string lowerInput = string.Empty;
            bool shouldRepeat;
            do
            {
                shouldRepeat = false;
                Console.WriteLine(">>>To get back to the menu press '#' + ENTER<<<");
                consoleInput = Console.ReadLine();
                lowerInput = consoleInput.ToLower();
                //if the customer has chosen to abort the action
                if (lowerInput == "#")
                    continue;

                if (lowerInput != "yes" && lowerInput != "no")
                {
                    Console.WriteLine($"Wrong input! Plese try again.");
                    shouldRepeat = true;
                    continue;
                }
                shouldRepeat = false;
            } while (shouldRepeat);
            return lowerInput;
        }

        private string GetNumberOfMeal(string name)
        {
            string consoleInput;
            bool shouldRepeat;
            do
            {
                shouldRepeat = false;
                Console.WriteLine(">>>To get back to the menu press '#' + ENTER<<<");
                consoleInput = Console.ReadLine();
                if (consoleInput == "#")
                    continue;
                if (!int.TryParse(consoleInput, out int inputNumber))
                {
                    Console.WriteLine("Wrong input! Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                if (inputNumber < 1 )
                {
                    Console.WriteLine("You must order at least one meal. Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                if (inputNumber > 20)
                {
                    Console.WriteLine("Maximum number of orders is 20. Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                shouldRepeat = false;
            } while (shouldRepeat);
            return consoleInput;
        }

        private string GetMealNo(int serialNo)
        {
            string consoleInput;
            bool shouldRepeat;
            do
            {
                shouldRepeat = false;
                Console.WriteLine(">>>To get back to the menu press '#' + ENTER<<<");
                consoleInput = Console.ReadLine();
                if (consoleInput == "#")
                    continue;
                if (!int.TryParse(consoleInput, out int inputNumber))
                {
                    Console.WriteLine("Wrong input! Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                if(inputNumber < 1 || inputNumber > serialNo - 1)
                {
                    Console.WriteLine("Wrong input! Please try again.");
                    shouldRepeat = true;
                    continue;
                }
                shouldRepeat = false;
            } while (shouldRepeat);
            return consoleInput;
        }

        public decimal CalculatePrice(Dictionary<tblMeal, int> meals)
        {
            if (!meals.Any())
                return 0;
            decimal? sum = 0;
            foreach (var m in meals)
            {
                sum += m.Key.Price * m.Value;
            }
            return (decimal)sum;

        }
    }
}
