import { Component, OnInit } from '@angular/core';
import {FormControl,Validator,Validators,FormGroup} from '@angular/forms';

import {Transaction} from '../imports/server.models';
import {TransactionService} from '../services/transaction.service'
import {errorObject} from '../imports/app.models'

@Component({
  selector: 'app-view-transactions',
  templateUrl: './view-transactions.component.html',
  styleUrls: ['./view-transactions.component.css']
})
export class ViewTransactionsComponent implements OnInit {

  transactions: Array<Transaction> = [];
  editTransaction: Transaction = null;
  notifications: Array<errorObject> = [];

  Name: FormControl;
  Amount: FormControl;
  Type: FormControl;
  Date: FormControl;
  editForm: FormGroup;

  constructor(private transactionService: TransactionService) { }

  ngOnInit(): void {
    this.getTransactionData()
  }

  getTransactionData(){
    this.transactions = [];

    this.transactionService.getTransactions().subscribe(
      data =>{
        this.setTransactions(data);
      },
      err =>{
        console.log("err: ", err)
      }
    )
  }

  setTransactions(data: any){

    for(let i = 0; i < data.length; i++){
      let date = new Date(data[i].date);

      let transaction = new Transaction(data[i].name,data[i].amount,date,data[i].id)
      transaction.Type = data[i].type;
      transaction.TransactionId = data[i].transactionId;

      this.transactions.push(transaction);
    }

    console.log(this.transactions);
  }

  showTransactionModal(transaction: Transaction){
    this.editTransaction = transaction;
    this.createTransactionForm();
  }

  createTransactionForm(){

    this.Name = new FormControl(this.editTransaction.Name, [Validators.required]);
    this.Amount = new FormControl(this.editTransaction.Amount, [Validators.required]);
    this.Type = new FormControl(this.editTransaction.Type,  [Validators.required]);
    this.Date = new FormControl(this.editTransaction.Date,  [Validators.required]);

    this.editForm = new FormGroup({
      name: this.Name,
      amount: this.Amount,
      type: this.Type,
      date: this.Date
    })
  }

  dateFilter(){
    let date: Date = new Date(this.editTransaction.Date);

    return date.toISOString().split('T')[0];
  }

  deleteTransaction(){
      if(confirm("Are you sure to delete this transaction? ")) {
        this.transactionService.deleteTransactions(this.editTransaction).subscribe(
          data =>{
            let message = `${this.editTransaction.Name} was deleted successfully`;
            this.notifications.push(new errorObject(this.notifications.length,true,message));
            this.getTransactionData();
          },
          err =>{
            if(typeof(err.error) != typeof(String))
            err.error = "An unexpected error occurred, please try again later"
  
          this.notifications.push(new errorObject(this.notifications.length,false,err.error));
          }
        );
      }
  }

  removeNotification(id: number){
    let newNotifications: Array<errorObject> = [];
    
    for(let i = 0; i < this.notifications.length; i++){
      if(id !== this.notifications[i].id)
      newNotifications.push(this.notifications[i])
    }

     this.notifications = newNotifications;
  }

  updateTransaction(){

    console.log('Updating')
    let date = this.Date.value;
    if(date == undefined || date == null){
      if(this.editTransaction.Date == null || this.editTransaction.Date == undefined)
      {
        date = Date.now();
      }
      else{
        date = this.editTransaction.Date;
      }
    }

    date = new Date(this.Date.value);

    let transaction = new Transaction(this.Name.value, this.Amount.value,date,this.editTransaction.Id);
    transaction.TransactionId = this.editTransaction.TransactionId;
    transaction.Type = this.Type.value;

    this.transactionService.updateTransaction(transaction).subscribe(
      data =>{
        let message = `${this.editTransaction.Name} was editied successfully`;
        this.notifications.push(new errorObject(this.notifications.length,true,message));
        this.getTransactionData();
      },
      err =>{
        if(typeof(err.error) != typeof(String))
        err.error = "An unexpected error occurred, please try again later"

      this.notifications.push(new errorObject(this.notifications.length,false,err.error));
      }
    );
  }

}
