using CASH_Masters;
using Microsoft.Extensions.Configuration;
using System.IO;

// See https://aka.ms/new-console-template for more information


// 1. Configure and build the configuration object
// This tells the application where to find appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


// 2. Read the "CurrencySettings" section and map it to your CurrencySettings class
var currencySettings  = configuration.GetSection("CurrencySettings").Get<CurrencySettings>();

// 3. Initialize your global static class with the loaded data
try
{

    ConfiguracionMonedaGlobal.Initialize(currencySettings);
    Console.WriteLine($"Loaded currency settings for: {ConfiguracionMonedaGlobal.CodeContry}");
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Data error in appsettings.json: {ex.Message}");
    return;    // Terminate the application if initialization fails
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error de configuración: {ex.Message}");
    return; // Termina la aplicación si la inicialización falla
}


// Now you can proceed with your application logic,
// for example, by asking the cashier for the price and payment:



Console.WriteLine("Welcome to the CASH Masters POS system!");

decimal itemPrice;


while (true) 
{
    Console.WriteLine("Please enter the item's price");
    if (decimal.TryParse(Console.ReadLine(), out itemPrice) && itemPrice >= 0)
    {
        break;
    }

    Console.WriteLine("The price must be a positive number.");
    
}

decimal customerPayment;
while (true)
{
    Console.WriteLine("Enter the amount paid by the customer:.");
    if (decimal.TryParse(Console.ReadLine(), out customerPayment) && customerPayment >= 0)
    {
        break;
    }

    Console.WriteLine("Invalid payment amount. Please enter a positive number.");

}

// Call your change calculation logic
var calculator = new ChangeCalculation(ConfiguracionMonedaGlobal.Denominations);


try {

    Dictionary<decimal, int> detailedChange = calculator.changeCalculator(itemPrice, customerPayment);


    Console.WriteLine("\n--- TRANSACTION SUMMARY ---");
    Console.WriteLine($"Price: {itemPrice:C}");
    Console.WriteLine($"Paid:  {customerPayment:C}");
    Console.WriteLine($"Change: {(customerPayment - itemPrice):C}");
    Console.WriteLine("------------------------------");


    if (detailedChange.Count == 0 && (itemPrice - customerPayment) == 0)
    {
        Console.WriteLine("No change needed. Exact payment received.");
    }
    else if (detailedChange.Count == 0 && (itemPrice - customerPayment) > 0)
    {
        Console.WriteLine("Could not calculate change with the given denominations.");
        Console.WriteLine($"Remaining amount to return: {(itemPrice - customerPayment):C}");
    }
    else
    {
        Console.WriteLine("Optimal change to return:");
        foreach (var denominacion in ConfiguracionMonedaGlobal.Denominations) // Itera sobre las denominaciones ordenadas
        {
            if (detailedChange.ContainsKey(denominacion) && detailedChange[denominacion] > 0)
            {
                Console.WriteLine($"{detailedChange[denominacion]} x {denominacion:C}");
            }
        }
    }

}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid input: {ex.Message}");
}
catch(InvalidOperationException ex)
{
    Console.WriteLine($"Could not calculate change. Please try again {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}   

Console.WriteLine("Thank you for using CASH Masters POS system!");
Console.ReadLine();



