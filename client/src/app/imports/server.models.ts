export class User {

    Email: string;
    Name: string;
    Password: String;
    Id: number;
    Role: string;

    constructor(Email: string ){
        this.Email = Email;
    }
}

export class Transaction {

    TransactionId: number;
    Amount: number;
    Name: string;
    Date: Date;
    Id: number; //Users Id
    Type: string;
    DateString: string; //M/D/Y

    constructor(name:string, amount: number, date: Date, userId: number){
        this.Name = name;
        this.Amount = amount;
        this.Date = date;
        this.Id =  userId;
        this.DateString = this.Date.getMonth() + '/' + (this.Date.getDay()-1) + '/' + this.Date.getFullYear(); //-1 because days were being incremented by 1
    }

    getFormattedDate(){
        return this.Date.toDateString();
    }
}

export class JwtToken{
    aud: string;
    exp: Date;
    iss: string;
}

export class Analytics{
    MostCommonTransactionType: string;
    Demographics: Array<SpendingDemographics> = [];
    TransactionCount: number;
    BiggestTransactionByAmount: BiggestTransaction;
}

export class SpendingDemographics{
    Type: string;
    NumberOfTransaction: number;
    MoneySpent: number;

    constructor(Type: string, NumberOfTransaction: number, MoneySpent: number){
        this.Type =Type;
        this.NumberOfTransaction = NumberOfTransaction;
        this.MoneySpent = MoneySpent;
    }
}


export class BiggestTransaction{
    Type: string;
    AmountSpent: number;

    constructor(Type: string, AmountSpent: number){
        this.Type = Type;
        this.AmountSpent = AmountSpent;
    }
}