using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CASH_Masters
{
    public class ChangeCalculation
    {

        /// <summary>
        /// Calculates the optimal change (minimum number of bills and coins) to return to the customer.
        /// </summary>
        /// <param name="itemPrice">The total price of the item(s) being purchased.</param>
        /// <param name="customerPayment">The total amount of money provided by the customer.</param>
        /// <returns>
        /// A dictionary where the key is the denomination (decimal) and the value is the quantity (int) of that denomination to return.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the payment amount is insufficient.</exception>
        /// <exception cref="InvalidOperationException">Thrown if exact change cannot be made with the configured denominations,
        /// or if global denominations have not been initialized.</exception>
        private readonly List<decimal> _denominations;


        public ChangeCalculation(List<decimal> denominations)
        {

            if (denominations == null || !denominations.Any()) { 
                throw new ArgumentNullException("Denominations cannot be null or empty.", nameof(denominations));
            }
            _denominations = denominations.OrderByDescending(x => x).ToList();  
        }
        
        public Dictionary<decimal, int> changeCalculator(decimal itemPrice, decimal  customerPayment)
        {


            if (!ConfiguracionMonedaGlobal.IsInitialized)
            {
                throw new InvalidOperationException($"Global currency denominations have not been initialized. Please ensure ConfiguracionMonedaGlobal.Initialize() is called at application startup.");
            }

            // 2. Validate that payment is sufficient
            if (customerPayment < itemPrice)
            {
                throw new ArgumentException($"The customer's payment is insufficient to cover the item's price.");
            }

            decimal changeDue = customerPayment - itemPrice;
            var result = new Dictionary<decimal, int>();

            // If no change needs to be returned, return an empty dictionary.
            if (changeDue == 0)
            {
                return result;
            }

            // 3. Get denominations sorted from highest to lowest from global configuration
            // Denominations will already be sorted because we configured them this way in Initialize()
            var denominations = _denominations;

            // 4. Implement optimal change algorithm (Greedy Algorithm)
            
            foreach (var denomination in denominations)
            {
                // Ensure no division by zero if a denomination is 0 (which shouldn't happen, but it's good practice)
                if (denomination <= 0) continue;

                // Calculate how many times this denomination fits into remaining change
                int count = (int)(changeDue / denomination);

                if (count > 0)
                {
                    // Add the count of this denomination to the result

                    result[denomination] = count;
                    // !!! CRITICAL FIX HERE: MUST BE MULTIPLICATION (*) NOT SUBTRACTION (-) !!!
                    changeDue -= (count * denomination);
                }

                // If pending change is zero (or very close to zero due to possible decimal imprecision),
                // we can stop the loop.
                if (Math.Abs(changeDue) < 0.0001m) // Pequeña tolerancia para decimales muy pequeños
                {
                    changeDue = 0; // Set to zero to ensure
                    break; // If no pending change remains, exit loop
                }
            }

            //5.Verify if all change could be given.
            // !!! CRITICAL FIX HERE: THIS VERIFICATION MUST BE OUTSIDE THE LOOP !!!
            if (changeDue > 0)
            {
                throw new InvalidOperationException($"Exact change could not be provided. Remaining change due: {changeDue:C}. Please check the configured denominations.");
            }

            return result;

        }
    }
}
