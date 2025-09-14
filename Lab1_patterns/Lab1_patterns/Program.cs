
using Lab1_patterns;

class Program
{
    /// <summary>
    /// Reads an integer from the console with a prompt. Repeats until a valid integer is entered.
    /// </summary>
    /// <param name="hint">Prompt text shown before reading input.</param>
    /// <returns>The parsed integer value.</returns>
    static int ReadInt(string hint)
    {
        while (true)
        {
            Console.Write(hint);
            if (int.TryParse(Console.ReadLine(), out int value)) return value;
            else Console.WriteLine("Enter an integer.");
        }
    }

    /// <summary>
    /// Reads a double from the console with a prompt. Repeats until a valid number is entered.
    /// Replaces '.' with ',' to support locales using a comma as the decimal separator.
    /// </summary>
    /// <param name="hint">Prompt text shown before reading input.</param>
    /// <returns>The parsed double value.</returns>
    static double ReadDouble(string hint)
    {
        while (true)
        {
            Console.Write(hint);
            if (double.TryParse((Console.ReadLine() ?? "").Replace('.', ','), out double value)) return value;
            else Console.WriteLine("Enter a number.");
        }
    }

    /// <summary>
    /// Operators registered in the app, keyed by operator ID.
    /// </summary>
    static Dictionary<int, Operator> operators = new();

    /// <summary>
    /// Human-friendly operator names keyed by operator ID.
    /// </summary>
    static Dictionary<int, string> operatorNames = new();

    /// <summary>
    /// Customers registered in the app, keyed by customer ID.
    /// </summary>
    static Dictionary<int, Customer> customers = new();

    /// <summary>
    /// Application entry point. Presents a console menu to manage operators and customers,
    /// perform calls/messages/data usage, and handle billing operations.
    /// </summary>
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU ===\n");
            Console.WriteLine("1. Add operator");
            Console.WriteLine("2. Add customer");
            Console.WriteLine("3. List operators");
            Console.WriteLine("4. List customers");
            Console.WriteLine("5. Call (talk)");
            Console.WriteLine("6. Message");
            Console.WriteLine("7. Internet (connection)");
            Console.WriteLine("8. Pay bill");
            Console.WriteLine("9. Change customer's operator");
            Console.WriteLine("10. Change customer's limit");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoose: ");

            var choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1": // add operator

                    Console.Write("\nOperator name: ");
                    string operatorName = Console.ReadLine() ?? "Operator";
                    double talk = ReadDouble("\nTalking charge (per minute): ");
                    double message = ReadDouble("\nMessage cost (per SMS): ");
                    double network = ReadDouble("\nNetwork charge (per MB): ");
                    double discount = ReadDouble("\nDiscount rate (%): ");

                    var op = new Operator(talk, message, network, discount);
                    operators[op.Id] = op;
                    operatorNames[op.Id] = operatorName;
                    Console.WriteLine($"\nOperator №{op.Id} {operatorName} added.");
                    break;

                case "2": // add customer

                    Console.Write("\nCustomer name: ");
                    string name = Console.ReadLine() ?? "Customer";
                    int age = ReadInt("Age: ");
                    double limit = ReadDouble("Bill limit: ");

                    var c = new Customer(name, age, limit);
                    customers[c.Id] = c;
                    Console.WriteLine($"Customer №{c.Id} {name} added.");
                    break;

                case "3": // list operators

                    if (operators.Count == 0) { Console.WriteLine("No operators."); break; }
                    foreach (var kv in operators)
                    {
                        var oper = kv.Value;
                        Console.WriteLine($"№{oper.Id}: {operatorNames[oper.Id]} | Talk = {oper.TalkingCharge}, Message = {oper.MessageCost}, Network = {oper.NetworkCharge}, Discount = {oper.DiscountRate}%");
                    }
                    break;

                case "4": // list customers

                    if (customers.Count == 0) { Console.WriteLine("No customers."); break; }
                    foreach (var kv in customers)
                    {
                        var customer = kv.Value;
                        int activeOp = customer.ActiveOperator?.Id ?? -1;
                        string activeOpName = operatorNames.ContainsKey(activeOp) ? operatorNames[activeOp] : "(none)";
                        Console.WriteLine($"№{customer.Id}: {customer.Name} | Age = {customer.Age} | Operator = №{activeOp} {activeOpName} | Debt = {Operator.bills[customer.Id].CurrentDebt} | Limit = {Operator.bills[customer.Id].LimitingAmount}");
                    }
                    break;

                case "5": // call

                    if (customers.Count < 2) { Console.WriteLine("Need at least 2 customers."); break; }

                    int callerId = ReadInt("Caller customer ID: ");
                    int receiverCallId = ReadInt("Receiver customer ID: ");

                    if (!customers.TryGetValue(callerId, out var from) || !customers.TryGetValue(receiverCallId, out var to)) { Console.WriteLine("Bad IDs."); break; }
                    int minutes = ReadInt("Minutes: ");

                    from.Talk(minutes, to);
                    break;

                case "6": // message

                    if (customers.Count < 2) { Console.WriteLine("Need at least 2 customers."); break; }

                    int senderId = ReadInt("Sender customer ID: ");
                    int receiverMessageId = ReadInt("Receiver customer ID: ");

                    if (!customers.TryGetValue(senderId, out var sender) || !customers.TryGetValue(receiverMessageId, out var receiverMessage)) { Console.WriteLine("Bad IDs."); break; }

                    int quantityMessage = ReadInt("Quantity: ");
                    sender.Message(quantityMessage, receiverMessage);
                    break;

                case "7": // internet

                    if (customers.Count == 0) { Console.WriteLine("No customers."); break; }

                    int customerId = ReadInt("Customer ID: ");
                    if (!customers.TryGetValue(customerId, out var cust)) { Console.WriteLine("Bad ID."); break; }

                    double mb = ReadDouble("MB: ");
                    cust.Connection(mb);
                    break;

                case "8": // pay

                    if (customers.Count == 0) { Console.WriteLine("No customers."); break; }

                    int payId = ReadInt("Customer ID: ");
                    if (!customers.TryGetValue(payId, out var payCustomer)) { Console.WriteLine("Bad ID."); break; }

                    double sum = ReadDouble("Pay amount: ");
                    Operator.bills[payCustomer.Id].Pay(sum);
                    break;

                case "9": // change active operator

                    if (customers.Count == 0 || operators.Count == 0) { Console.WriteLine("Add customers/operators first."); break; }

                    int changeId = ReadInt("Customer ID: ");
                    if (!customers.TryGetValue(changeId, out var ch)) { Console.WriteLine("Bad ID."); break; }

                    foreach (var kv in operators) Console.WriteLine($" №{kv.Key}: {operatorNames[kv.Key]}");

                    int newOperator = ReadInt("New operator ID: ");

                    if (!ch.ChangeActiveOperatorById(newOperator)) Console.WriteLine("Operator not found.");
                    else Console.WriteLine("Operator changed.");
                    break;

                case "10": // change limit

                    if (customers.Count == 0) { Console.WriteLine("No customers."); break; }

                    int changeLimitId = ReadInt("Customer ID: ");
                    if (!customers.TryGetValue(changeLimitId, out var changeLimit)) { Console.WriteLine("Bad ID."); break; }
                    double newLimit = ReadDouble("New limit: ");
                    Operator.bills[changeLimit.Id].ChangeTheLimit(newLimit);
                    break;

                default:
                    Console.WriteLine("Unknown choice.");
                    break;
            }

            Console.WriteLine();
        }
    }

}