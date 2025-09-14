using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_patterns
{
    internal class Bill
    {
        public double LimitingAmount {  get; private set; }
        public double CurrentDebt { get; private set; }

        public Bill(double limit)
        {
            this.LimitingAmount = limit;
            this.CurrentDebt = 0;
            Console.WriteLine("\nCreated bill!\n");
        }
        public bool Check(double amount) => CurrentDebt + amount <= LimitingAmount;

        public void Add(double amount)
        {
            CurrentDebt += amount;
            Console.WriteLine($"Current debt after adding: {CurrentDebt}\n");
        }
        public void Pay(double amount)
        {
            double newDebt = CurrentDebt - amount;
            double overpay = newDebt < 0 ? -newDebt : 0;
            CurrentDebt = newDebt > 0 ? newDebt : 0;

            Console.WriteLine($"Overpay: {overpay}\n");
            Console.WriteLine($"Current debt: {CurrentDebt}\n");
        }
        public void ChangeTheLimit(double amount)
        {
            LimitingAmount = amount;
            Console.WriteLine($"Current limit: {LimitingAmount}\n");
        }

        public bool TryCharge(double amount)
        {
            if (!Check(amount))
            {
                Console.WriteLine("Limiting amount exceeded!\n");
                return false;
            }
            Console.WriteLine("Limiting amount NOT exceeded!\n");
            Add(amount); 
            return true;
        }
    }
}
