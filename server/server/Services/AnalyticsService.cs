using server.Entities;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class AnalyticsService
    {
        public Analytics GetAllAnalytics(List<TransactionObject> userTransactions)
        {

            var analytics = new Analytics();

            int transactionCount = GetTransactionCount(userTransactions);
            analytics.TransactionCount = transactionCount;

            //breaking from service to prevent other analytics being calculated
            if (transactionCount == 0)
                return analytics;

            Analytics mostCommonAndTypesPurchased = GetMostCommonAndTypesPurchased(userTransactions);

            analytics.PurchasedTypes = mostCommonAndTypesPurchased.PurchasedTypes;
            analytics.MostCommonTransactionType = mostCommonAndTypesPurchased.MostCommonTransactionType;
            
          

            return analytics;
        }

        public int GetTransactionCount(List<TransactionObject> userTransactions)
        {
            if (userTransactions == null || userTransactions.Count() == 0)
            {
                return 0;
            }
            else
            {
                return userTransactions.Count;
            }
        }

        public Analytics GetMostCommonAndTypesPurchased(List<TransactionObject> userTransactions)
        {
            var analytics = new Analytics();
            var map = new Dictionary<string, AmountOfTypePurchased>();
            HashSet<string> typesAdded = new HashSet<string>();

            string mostPurchasedType = null; //variables to find most purchased type while already iterating transactions
            int mostPurchasedTypeValue = 0;

            foreach (TransactionObject transaction in userTransactions)
            {
                string type = transaction.Type.ToLower();

                if (typesAdded.Contains(type))
                {
                    AmountOfTypePurchased amountOfTypePurchased = map[type];
                    amountOfTypePurchased.AmountPurchased = amountOfTypePurchased.AmountPurchased + 1;
                    map[type] = amountOfTypePurchased;

                    if (amountOfTypePurchased.AmountPurchased > mostPurchasedTypeValue)
                    {
                        mostPurchasedType = amountOfTypePurchased.Type;
                        mostPurchasedTypeValue = amountOfTypePurchased.AmountPurchased;
                    }
                    else if (amountOfTypePurchased.AmountPurchased == mostPurchasedTypeValue)
                    {
                        mostPurchasedType = mostPurchasedType + ',' + amountOfTypePurchased.Type;
                    }
                }
                else
                {
                    typesAdded.Add(type);
                    map.Add(type, new AmountOfTypePurchased(type, 1));

                    if (mostPurchasedTypeValue == 0)
                    {
                        mostPurchasedTypeValue = 1;
                        mostPurchasedType = type;
                    }
                }
            }

            analytics.MostCommonTransactionType = mostPurchasedType;

            analytics.PurchasedTypes = new AmountOfTypePurchased[map.Count];

            for(int i = 0; i < map.Count; i++)
            {
                var item = map.ElementAt(i);
                analytics.PurchasedTypes[i] = item.Value;
            }

            return analytics;
        }
    }
}
