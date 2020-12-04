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
        public SpendingDemographics[] Demographics{ get; set; }
        public BiggestTransaction BiggestTransactionByAmount { get; set; }

    }
    public class SpendingDemographics
    {
        public string Type { get; set; }
        public double MoneySpent { get; set; }
        public int NumberOfTransactions { get; set; }

        public SpendingDemographics(string Type, double MoneySpent, int NumberOfTransactions)
        {
            this.Type = Type;
            this.MoneySpent = MoneySpent;
            this.NumberOfTransactions = NumberOfTransactions;
        }

    }

    public class BiggestTransaction{
        public string Type { get; set; }
        public double AmountSpent { get; set; }

        public BiggestTransaction(string Type, double AmountSpent)
        {
            this.Type = Type;
            this.AmountSpent = AmountSpent;
        }
}

}
