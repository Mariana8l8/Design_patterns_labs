using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_patterns
{
    internal class Customer
    {
        private static int customerId;

        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public Operator? ActiveOperator { get; set; }
        public Dictionary<int, Operator> operators { get; set; } = new Dictionary<int, Operator>();
        public Customer(string name, int age, double limitingAmount)
        {
            Console.WriteLine("\nCreated customer!\n");
            this.Id = customerId++;
            this.Name = name;
            this.Age = age;
            this.ActiveOperator = null;
        }

        public void SetActiveOperatorById(int operatorId)
        {
            if (ActiveOperator == null)
            {
                ActiveOperator = operators[operatorId];
                Console.WriteLine("The active operator was successfully seted\n");
            }
            else
            {
                Console.WriteLine($"The active operator is already set \"{ActiveOperator}\". Change it if necessary.");
            }
        }
        public void ChangeActiveOperator(Operator op) { ActiveOperator = op; Console.WriteLine("Operator successfully changed!\n"); }
        public bool ChangeActiveOperatorById(int operatorId)
        {
            var op = operators[operatorId];
            if (op != null)
            {
                ActiveOperator = op;
                Console.WriteLine("The operator was successfully changed!\n");
                return true;
            }
            else { Console.WriteLine("The operator was not changed!\n"); return false; }
        }

        public void Talk(int min, Customer other)
        {
            if (ActiveOperator == null) { Console.WriteLine("To start, set the active operator!\n"); }
            else
            {
                double cost = ActiveOperator.CalculateTalkingCost(min, this, other);
                if (Operator.bills[Id].TryCharge(cost)) Console.WriteLine("The call is successful!\n");
                else Console.WriteLine("The call is unsuccessful!\n");
            }
        }
        public void Message(int quantity, Customer other)
        {
            if (ActiveOperator == null) { Console.WriteLine("To start, set the active operator!\n"); }
            else
            {
                double cost = ActiveOperator.CalculateMessageCost(quantity, this, other);
                if (Operator.bills[Id].TryCharge(cost)) Console.WriteLine("The message was sent successfully!\n");
                else Console.WriteLine("The message was not sent!\n");
            }
        }
        public void Connection(double amount)
        {
            if (ActiveOperator == null) { Console.WriteLine("To start, set the active operator!\n"); }
            else
            {
                double cost = ActiveOperator.CalculateNetworkCost(amount, this);
                if (Operator.bills[Id].TryCharge(cost)) Console.WriteLine("The connection is successful!\n");
                else Console.WriteLine("The connection is unsuccessful!\n");
            }
        }
    }
}
