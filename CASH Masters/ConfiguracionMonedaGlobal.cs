using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace CASH_Masters
{

    /// <summary>
    /// Provides access to globally configured currency denominations for the application.
    /// This class MUST be initialized exactly once from the application's entry point.
    /// </summary>
    public static class ConfiguracionMonedaGlobal
    {
        public static List<decimal> Denominations { get; private set; }
        public static string CodeContry { get; private set; }

        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes the global currency configuration.
        /// This method should be called EXACTLY ONCE at application startup.
        /// </summary>
        /// <param name="currencySettings">Object containing currency configuration read from appsettings.json.</param>

        public static void Initialize(CurrencySettings currencySettings)
        {

            if (_isInitialized) {

                throw new InvalidOperationException("The global currency configuration has already been initialized. It cannot be initialized twice.");
            }

            if (currencySettings == null || !currencySettings.Denominations.Any()) {
                throw new ArgumentNullException("The currency configuration or its denominations cannot be null or empty.");
            }


            CodeContry = currencySettings.CountryCode;

            // Ensure denominations are sorted from highest to lowest.
            Denominations = new List<decimal>(currencySettings.Denominations.OrderByDescending(d => d));
            _isInitialized = true;


           

        }

        /// <summary>
        /// Checks whether the global currency configuration has been initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

    }
}
