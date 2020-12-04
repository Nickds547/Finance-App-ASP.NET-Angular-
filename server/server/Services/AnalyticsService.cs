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
        public Analytics GetBaseAnalytics(List<TransactionObject> userTransactions)
        {

            var analytics = new Analytics();

            int transactionCount = GetTransactionCount(userTransactions);
            analytics.TransactionCount = transactionCount;

            //breaking from service to prevent other analytics being calculated
            if (transactionCount == 0)
                return analytics;

            Analytics mostCommonAndDemographics = GetMostCommonAndDemographics(userTransactions);

            analytics.Demographics = mostCommonAndDemographics.Demographics;
            analytics.MostCommonTransactionType = mostCommonAndDemographics.MostCommonTransactionType;
            analytics.BiggestTransactionByAmount = mostCommonAndDemographics.BiggestTransactionByAmount;




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

        public Analytics GetMostCommonAndDemographics(List<TransactionObject> userTransactions)
        {
            var analytics = new Analytics();
            var map = new Dictionary<string, SpendingDemographics>();
            HashSet<string> typesAdded = new HashSet<string>();

            string mostPurchasedType = null; //variables to find most purchased type while already iterating transactions
            int highestTransactionCount = 0;
            double currentMostAmountSpent = 0;
            string typeWithHighestTransactionAmount = null;

            foreach (TransactionObject transaction in userTransactions)
            {
                string type = transaction.Type.ToLower();

                //if type has been used before, increment relevant spending demographics and check against highestTransactionCount
                if (typesAdded.Contains(type))
                {
                    SpendingDemographics spendingDemographics = map[type];
                    spendingDemographics.NumberOfTransactions = spendingDemographics.NumberOfTransactions + 1;
                    spendingDemographics.MoneySpent = spendingDemographics.MoneySpent + transaction.Amount;
                    map[type] = spendingDemographics;

                    if (spendingDemographics.NumberOfTransactions > highestTransactionCount)
                    {
                        mostPurchasedType = type;
                        highestTransactionCount = spendingDemographics.NumberOfTransactions;
                    }
                    else if (spendingDemographics.NumberOfTransactions == highestTransactionCount)
                    {
                        mostPurchasedType = mostPurchasedType + ',' + spendingDemographics.Type;
                    }

                }
                else //if trpe has not been used before, create a new SpendingDemographics object and store it in the map
                {
                    typesAdded.Add(type);
                    map.Add(type, new SpendingDemographics(type, transaction.Amount, 1));

                    if (highestTransactionCount == 0)
                    {
                        highestTransactionCount = 1;
                        mostPurchasedType = type;
                    }
                }

                if (transaction.Amount > currentMostAmountSpent)
                {
                    currentMostAmountSpent = transaction.Amount;
                    typeWithHighestTransactionAmount = type;
                       
                }
            }

            analytics.MostCommonTransactionType = mostPurchasedType;
            BiggestTransaction biggestTransaction = new BiggestTransaction(typeWithHighestTransactionAmount, currentMostAmountSpent);
            analytics.BiggestTransactionByAmount = biggestTransaction;

            analytics.Demographics = new SpendingDemographics[map.Count];

            for(int i = 0; i < map.Count; i++)
            {
                var item = map.ElementAt(i);
                analytics.Demographics[i] = item.Value;
            }

            return analytics;
        }

        private (double, string) CheckAmountSpent(double currentMostAmountSpent, double transactionAmount,string typeWithHighestTransactionAmount, string transactionType)
        {
            if(transactionAmount > currentMostAmountSpent)
            {
                return (transactionAmount, transactionType);
            }
            else if(transactionAmount == currentMostAmountSpent && typeWithHighestTransactionAmount != transactionType)
            {
                return (currentMostAmountSpent, typeWithHighestTransactionAmount);
            }
            else
            {
                return (currentMostAmountSpent, typeWithHighestTransactionAmount);
            }

        }
    }
}
