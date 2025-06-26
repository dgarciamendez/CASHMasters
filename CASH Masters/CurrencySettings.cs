using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASH_Masters
{
    public class CurrencySettings
    {

        public string CountryCode { get; set; }
        public List<decimal> Denominations { get; set; }
    }
}
