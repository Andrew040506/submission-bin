using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    abstract class Vehicle
    {
        protected double plateNumber;
        protected double maxCapacity;
        protected double currentLoad;

        public double MaxCapacity => maxCapacity;

        protected Vehicle(double currentLoad)
        {
            this.currentLoad = currentLoad;
        }

        private bool loadPackage(double weight)
        {
            if (currentLoad + weight <= maxCapacity)
            {
                currentLoad += weight;
                Console.WriteLine($"Package loaded. Current load: {currentLoad} kg.");
                return true;
            }
            else
            {
                Console.WriteLine("Cannot load package. Exceeds maximum capacity.");
                return false;
            }
        }

        public abstract double CalculateDeliveryFee();
    }

    class DeliveryTruck : Vehicle
    {
        private int numAxles { get; set; }

        public DeliveryTruck(double currentLoad) : base(currentLoad)
        {
            maxCapacity = 1000;
        }

        public override double CalculateDeliveryFee()
        {
            return currentLoad * 2.50;
        }
    }

    class Motorcycle : Vehicle
    {
        private bool hasInsulatedBox { get; set; }

        public Motorcycle(double currentLoad) : base(currentLoad)
        {
            maxCapacity = 35;
        }

        public override double CalculateDeliveryFee()
        {
            return 15;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> transactionHistory = new();
            bool keepRunning = true;

            while (keepRunning)
            {
                string customerName = ReadRequiredText("Customer Name: ");
                bool isPremium = ReadSubscriptionStatus();
                string vehiclePreference = ReadVehiclePreference();
                double packageWeight = ReadWeightForVehicle(vehiclePreference);

                Customer customer = new Customer(customerName, isPremium);

                Vehicle vehicle = vehiclePreference == "1"
                    ? new DeliveryTruck(packageWeight)
                    : new Motorcycle(packageWeight);

                double baseDeliveryFee = vehicle.CalculateDeliveryFee();
                double finalDeliveryFee = customer.IsPremium ? baseDeliveryFee * 0.75 : baseDeliveryFee;

                string transactionRecord =
                    $"Customer: {customer.Name}, " +
                    $"Subscription: {(customer.IsPremium ? "Premium" : "Standard")}, " +
                    $"Vehicle: {(vehiclePreference == "1" ? "Delivery Truck" : "Motorcycle")}, " +
                    $"Weight: {packageWeight} kg, " +
                    $"Fee: {finalDeliveryFee:F2}";

                transactionHistory.Add(transactionRecord);

                Console.WriteLine();
                Console.WriteLine("Transaction successful.");
                if (customer.IsPremium)
                {
                    Console.WriteLine($"Premium discount applied: 25% (Base Fee: {baseDeliveryFee:F2})");
                }

                Console.WriteLine(transactionRecord);
                Console.WriteLine();

                keepRunning = AskNextAction();
            }

            Console.WriteLine();
            Console.WriteLine("Final Transaction Report:");

            if (transactionHistory.Count == 0)
            {
                Console.WriteLine("No successful transactions recorded.");
            }
            else
            {
                for (int i = 0; i < transactionHistory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {transactionHistory[i]}");
                }
            }
        }

        static string ReadRequiredText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("Input cannot be empty.");
            }
        }

        static bool ReadSubscriptionStatus()
        {
            while (true)
            {
                Console.Write("Subscription Status (Premium/Standard): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input.Equals("Premium", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (input.Equals("Standard", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                Console.WriteLine("Invalid input. Enter Premium or Standard.");
            }
        }

        static string ReadVehiclePreference()
        {
            while (true)
            {
                Console.Write("Vehicle Preference (1 for Delivery truck, 2 for Motorcycle): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input == "1" || input == "2")
                {
                    return input;
                }

                Console.WriteLine("Invalid input. Enter 1 or 2.");
            }
        }

        static double ReadWeightForVehicle(string vehiclePreference)
        {
            double maxCapacity = vehiclePreference == "1" ? 1000 : 35;

            while (true)
            {
                Console.Write($"Package Weight (max {maxCapacity} kg): ");
                string input = Console.ReadLine() ?? string.Empty;

                if (!double.TryParse(input, out double weight) || weight <= 0)
                {
                    Console.WriteLine("Invalid package weight. Enter a number greater than 0.");
                    continue;
                }

                if (weight > maxCapacity)
                {
                    Console.WriteLine($"Weight exceeds vehicle limit of {maxCapacity} kg.");
                    continue;
                }

                return weight;
            }
        }

        static bool AskNextAction()
        {
            while (true)
            {
                Console.Write("Choose next action (1 for Adding another transaction / 2 for Exit): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input.Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (input.Equals("2", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                Console.WriteLine("Invalid input. Type exactly: 1 or 2.");
            }
        }
    }
}

