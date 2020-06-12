using DAN_XXXI_Jasmina_Kostadinovic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XXXI_Jasmina_Kostadinovic.DBHaendlers
{
    class DataAccess
    {
        public List<tblMeal> LoadMeals()
        {
            using(var conn = new RestaurantEntities())
            {                
                var meals = new List<tblMeal>();
                if (conn.tblMeals.Any())
                    return conn.tblMeals.ToList();
                return meals;
            }
        }

        public List<tblOrder> LoadOrders()
        {
            using (var conn = new RestaurantEntities())
            {
                var orders = new List<tblOrder>();
                if (conn.tblOrders.Any())
                    return conn.tblOrders.ToList();
                return orders;
            }
        }

        public List<tblMealOrder> LoadOrdersAndMeals()
        {
            using (var conn = new RestaurantEntities())
            {
                var ordersAndMeals = new List<tblMealOrder>();
                if (conn.tblOrders.Any())
                    return conn.tblMealOrders.ToList();
                return ordersAndMeals;
            }
        }

        public tblOrder LoadOrder(int id)
        {
            using (var conn = new RestaurantEntities())
            {
                return conn.tblOrders.FirstOrDefault(o => o.OrderID == id);
            }
        }

        public void AddNewOrder(tblOrder order)
        {
            using (var conn = new RestaurantEntities())
            {
                conn.tblOrders.Add(order);
                conn.SaveChanges();
            }
        }

        public void AddNewOrderedMeal(tblMealOrder mealOrder)
        {
            using (var conn = new RestaurantEntities())
            {
                conn.tblMealOrders.Add(mealOrder);
                conn.SaveChanges();
            }
        }

        public void UpdateOrder(tblOrder updatedOrder)
        {
            using (var conn = new RestaurantEntities())
            {
                var order = conn.tblOrders.FirstOrDefault(o => o.OrderID == updatedOrder.OrderID);
                if(order != null)
                {
                    order.Price = updatedOrder.Price;
                    order.tblMealOrders = updatedOrder.tblMealOrders;
                    order.AddressOfRecipient = updatedOrder.AddressOfRecipient;
                    order.DateOfOrder = order.DateOfOrder;
                    conn.SaveChanges();
                }
            }
        }
        
    }
}
