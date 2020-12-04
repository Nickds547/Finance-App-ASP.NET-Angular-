using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Entities
{
    public class Analytics
    {
        public int TransactionCount { get; set; }
        public string MostCommonTransactionType { get; set; }
        public AmountOfTypePurchased[] PurchasedTypes{ get; set; }
    }
    public class AmountOfTypePurchased
    {
        public AmountOfTypePurchased(string Type, int AmountPurchased)
        {
            this.Type = Type;
            this.AmountPurchased = AmountPurchased;
        }
        public string Type { get; set; }
        public int AmountPurchased { get; set; }
    }

}
