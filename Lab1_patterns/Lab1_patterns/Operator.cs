using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_patterns
{
    internal class Operator
    {
        public int Id { get; set; }
        private static int nextId; 
        public double TalkingCharge { get; }
        public double MessageCost { get; }
        public double NetworkCharge { get; }
        public double DiscountRate { get; set; }
        private static bool AgeDiscount(int age) => age < 18 || age > 65;

        public static Dictionary<int, Bill> bills { get; set; } = new Dictionary<int, Bill>();
        public Operator(double talkingCharge, double messageCost, double networkCharge, double discountRate)
        {
            this.Id = nextId++;
            this.TalkingCharge = talkingCharge;
            this.MessageCost = messageCost;
            this.NetworkCharge = networkCharge;
            this.DiscountRate = discountRate;
            Console.WriteLine("\nCreated operator!\n");
        }
        
        public double CalculateTalkingCost(int min, Customer customer, Customer other)
        {
            double cost = min * TalkingCharge;
            if (customer.ActiveOperator == other.ActiveOperator && DiscountRate > 0 || AgeDiscount(customer.Age) && DiscountRate > 0)
            {
                cost *= (100 - DiscountRate) / 100.0;
                Console.WriteLine("Discount activated!\n");
            }
            Console.WriteLine($"Talking cost: {cost}\n");
            return cost;
        }
        public double CalculateMessageCost(int quantity, Customer customer, Customer other)
        {
            double cost = quantity * MessageCost;
            if (customer.ActiveOperator == other.ActiveOperator && DiscountRate > 0)
            {
                cost *= (100 - DiscountRate) / 100.0;
                Console.WriteLine("Discount activated!\n");
            }
            Console.WriteLine($"Message cost: {cost}\n");
            return cost;
        }
        public double CalculateNetworkCost(double amountMB, Customer customer)
        {
            double cost = amountMB * NetworkCharge;
            Console.WriteLine($"Network cost: {cost}\n");
            return cost;
        }

        public void ConnectionToTheOperator(Customer customer, double amountLimit)
        {
            if (customer.operators.ContainsKey(Id)) Console.WriteLine("Operator already connected!\n");
            else
            {
                customer.operators.Add(Id, this);
                Console.WriteLine($"The operator has been successfully connected to the {customer.Name}.\n");
                CreateBill(customer.Id, amountLimit);
            }
        }
        public Bill CreateBill(int id, double amountLimit) 
        {
            if (bills.ContainsKey(id))
            {
                Console.WriteLine("Bill already exists!");
                return bills[id];
            }
            else
            {
                var bill = new Bill(amountLimit);
                bills.Add(id, bill);
                Console.WriteLine("Bill successfully created!");
                return bill;
            }
        } 
    }
}
