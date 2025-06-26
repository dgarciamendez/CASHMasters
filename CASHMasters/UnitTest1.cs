using CASH_Masters;
using Xunit;
using System.Collections.Generic;
using System;
{
    
}
namespace CASHMasters
{
    public class UnitTest1
    {

        public UnitTest1()
        {

            var currencySettings = new CurrencySettings
            {
                CountryCode = "US",
                Denominations = new List<decimal> { 100.00m, 50.00m, 20.00m, 10.00m, 5.00m, 2.00m, 1.00m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m }

            };

            if (!ConfiguracionMonedaGlobal.IsInitialized)
            {
                ConfiguracionMonedaGlobal.Initialize(currencySettings);
            }


        }

        [Fact]
        public void ChangeCalculator_ShouldReturnCorrectChange_ForExactHundred()
        {
            // Arrange
            var denominations = new List<decimal> { 100.00m, 50.00m, 20.00m, 10.00m, 5.00m, 2.00m, 1.00m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m };
            var calculator = new ChangeCalculation(denominations);
            decimal itemPrice = 100m;
            decimal customerPayment = 200m;
            var expectedChange = new Dictionary<decimal, int> { { 100.00m, 1 } };

            // Act
            var result = calculator.changeCalculator(itemPrice, customerPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedChange.Count, result.Count);
            foreach (var entry in expectedChange)
            {
                Assert.True(result.ContainsKey(entry.Key), $"Falta la denominación esperada: {entry.Key}");
                Assert.True(entry.Value == result[entry.Key],
                            $"Conteo incorrecto para la denominación: {entry.Key}. Esperado: {entry.Value}, Obtenido: {result[entry.Key]}");
            }
        }


        [Fact]
        public void ChangeCalculator_ShouldReturnEmptyDictionary_ForExactPayment()
        {
            // Arrange
            var calculator = new ChangeCalculation(ConfiguracionMonedaGlobal.Denominations);
            decimal itemPrice = 50m;
            decimal customerPayment = 50m;

            // Act
            var result = calculator.changeCalculator(itemPrice, customerPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public void ChangeCalculator_ShouldThrowArgumentException_ForInsufficientPayment()
        {
            // Arrange
            var calculator = new ChangeCalculation(ConfiguracionMonedaGlobal.Denominations);
            decimal itemPrice = 100m;
            decimal customerPayment = 50m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.changeCalculator(itemPrice, customerPayment));
        }


        [Fact]
        public void ChangeCalculator_ShouldReturnCorrectChange_ForComplexAmount()
        {
            // Arrange
            var calculator = new ChangeCalculation(ConfiguracionMonedaGlobal.Denominations);
            decimal itemPrice = 12.34m;
            decimal customerPayment = 20.00m; // Cambio esperado: 7.66

            var expectedChange = new Dictionary<decimal, int>
            {
                { 5.00m, 1 },
                { 2.00m, 1 },
                { 0.50m, 1 },
                { 0.10m, 1 },
                { 0.05m, 1 },
                { 0.01m, 1 }
            };

            // Act
            var result = calculator.changeCalculator(itemPrice, customerPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedChange.Count, result.Count);
            foreach (var entry in expectedChange)
            {
                Assert.True(result.ContainsKey(entry.Key), $"Falta la denominación esperada: {entry.Key}");

                // --- APLICA LA MISMA CORRECCIÓN AQUÍ ---
                Assert.True(entry.Value == result[entry.Key],
                            $"Conteo incorrecto para la denominación: {entry.Key}. Esperado: {entry.Value}, Obtenido: {result[entry.Key]}");
            }
        }

        [Fact]
        public void ChangeCalculator_ShouldReturnCoinsOnly_ForSmallChange()
        {
            // Arrange
            var calculator = new ChangeCalculation(ConfiguracionMonedaGlobal.Denominations);
            decimal itemPrice = 0.99m;
            decimal customerPayment = 1.00m; // Cambio esperado: 0.01
            var expectedChange = new Dictionary<decimal, int> { { 0.01m, 1 } };

            // Act
            var result = calculator.changeCalculator(itemPrice, customerPayment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedChange.Count, result.Count);
            Assert.True(result.ContainsKey(0.01m));
            Assert.Equal(1, result[0.01m]);
        }


        
    }
}