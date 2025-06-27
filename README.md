# CASH Masters - Change Calculation System

## Project Overview

This project implements a Point-of-Sale (POS) system component for CASH Masters, focusing on calculating the optimal (minimum) number of bills and coins to return as change to a customer. The system is designed to be globally compatible, allowing for different currency denominations based on a global configuration.

## Features

* Calculates optimal change using a greedy algorithm.
* Supports global currency configuration with customizable denominations.
* Includes robust error handling for insufficient payments and inability to provide exact change.
* Demonstrates object-oriented principles and includes comprehensive unit tests.

## Important Note for Reviewers

Please be aware that the `main` branch of this repository might be empty. The complete and functional project code, including all features and tests, is located on the `master` branch.

You can view it directly here: https://github.com/dgarciamendez/CASHMasters/tree/master

If you clone the repository, please make sure to switch to the `master` branch:


git clone https://github.com/dgarciamendez/CASHMasters.git
cd CASHMasters # Navigate into the cloned repository
git checkout master

Prerequisites:
Before you begin, ensure you have the following installed on your system:

.NET SDK 6.0 or higher: Download .NET SDK
(If you use a newer version like .NET SDK 8.0, it will work perfectly.)

Getting Started
Follow these steps to get a copy of the project up and running on your local machine.

1. Clone the Repository
First, clone the project repository to your local machine:Please be aware that the `main` branch of this repository might be empty. The complete and functional project code, including all features and tests, is located on the `master` branch.

git clone https://github.com/dgarciamendez/CASHMasters.git
cd "CASH Masters" # Navigate into the solution folder
git checkout master

2. Restore Dependencies
Navigate to the root of the solution (where the .sln file is located) and restore the NuGet packages:
dotnet restore

3. Build the Project
Build the entire solution to compile all projects:
dotnet build


4. Run the Console Application
After a successful build, you can run the console application. Navigate to the main project directory (CASH Masters/CASH Masters relative to your solution root) and execute:

Bash

cd CASH\ Masters # If you are in the solution root
dotnet run
The application will prompt you to enter the item price and customer payment.

Configuration
The application uses appsettings.json for its base configuration and appsettings.Development.json for environment-specific overrides (e.g., for development purposes).

appsettings.json: Contains the default currency settings, including the CountryCode and a list of Denominations. This file holds the configuration that applies to all environments unless overridden.

JSON

{
  "CurrencySettings": {
    "CountryCode": "US",
    "Denominations": [ 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]
  }
}
appsettings.Development.json: This file can be used to override settings defined in appsettings.json specifically for the development environment. For example, you might define different denominations or logging settings here. When the application runs in the Development environment, settings from this file will take precedence over appsettings.json.

Example of appsettings.Development.json (if you were to use Mexican Pesos in development):

JSON

{
  "CurrencySettings": {
    "CountryCode": "MX",
    "Denominations": [ 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 ]
  }
}
The application loads these settings using Microsoft.Extensions.Configuration, automatically applying overrides based on the current environment.

Running Tests
Unit tests are provided to ensure the correctness and robustness of the change calculation logic.

1. Navigate to the Test Project
From the solution root directory, navigate to the test project:

Bash

cd CASHMASTER.Tests
2. Execute Unit Tests
Run all unit tests using the .NET CLI:
dotnet test


3. Check Code Coverage (Optional)
To measure code coverage, you can use dotnet-coverage and ReportGenerator.

First, ensure you have these tools installed globally:
dotnet tool install --global dotnet-coverage
dotnet tool install -g dotnet-reportgenerator-globaltool
Then, from the CASHMASTER.Tests directory, run:

dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml" --session-id "cash_masters_coverage"
reportgenerator -reports:coverage.xml -targetdir:coverage_report -reporttypes:Html


After these commands complete, open the index.html file located in the coverage_report directory in your web browser to view the detailed coverage report.

https://github.com/user-attachments/assets/9d34af63-ee78-410e-993b-aeaec59e08af









