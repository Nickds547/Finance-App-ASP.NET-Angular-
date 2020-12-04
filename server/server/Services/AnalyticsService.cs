﻿using server.Entities;
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

            analytics.TransactionCount = GetTransactionCount(userTransactions);

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
                return userTransactions.Count();
            }
        }

        public Analytics GetMostCommonAndTypesPurchased(List<TransactionObject> userTransactions)
        {
            var analytics = new Analytics();
            var map = new Dictionary<string, AmountOfTypePurchased>();
            HashSet<string> typesAdded = new HashSet<string>();

            string mostPurchasedType = null;
            int mostPurchasedValue = 0;

            foreach (TransactionObject transaction in userTransactions)
            {
                string type = transaction.Type.ToLower();

                if (typesAdded.Contains(type))
                {
                    AmountOfTypePurchased amountOfTypePurchased = map[type];
                    amountOfTypePurchased.AmountPurchased = amountOfTypePurchased.AmountPurchased + 1;
                    map[type] = amountOfTypePurchased;

                    if (amountOfTypePurchased.AmountPurchased > mostPurchasedValue)
                    {

                    }
                }
                else
                {
                    typesAdded.Add(type);
                    map.Add(type, new AmountOfTypePurchased(type, 1));

                    if (mostPurchasedValue == 0)
                    {
                        mostPurchasedValue = 1;
                        mostPurchasedType = type;
                    }
                }
            }

            analytics.MostCommonTransactionType = mostPurchasedType;


            int count = 0;
            foreach (KeyValuePair<string, AmountOfTypePurchased> entry in map)
            {
                analytics.PurchasedTypes[count] = entry.Value;
                count++;
            }

            return analytics;
        }
    }
}
