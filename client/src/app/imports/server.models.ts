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
    UserId: number; //Users Id

    constructor(name:string, amount: number, date: Date, userId: number){
        this.Name = name;
        this.Amount = amount;
        this.Date = date;
        this.UserId =  userId;
    }
}

export class JwtToken{
    aud: string;
    exp: Date;
    iss: string;
}