import { Component, OnInit } from '@angular/core';
import {FormControl,Validators,FormGroup, FormBuilder, Validator, FormArray} from '@angular/forms';
import {Transaction, User} from '../imports/server.models'
import {AuthService} from '../services/auth.service'
import {TransactionService} from '../services/transaction.service'
import {errorObject} from '../imports/app.models'
import {inParams} from '../imports/validations'

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {

  notification: errorObject = null;
  page: string;

  addTransactionForm = this.formBuilder.group({
    fields: new FormArray([])
  })

  user: User;

  get f() {return this.addTransactionForm.controls}
  get m() {return this.f.fields as FormArray}

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private transactionService: TransactionService) 
  { 
    this.page = "Add"
  }

  ngOnInit(): void {
    this.addField();
    this.user = this.authService.getUser();
  }

  addField = () =>{
    this.m.push(this.formBuilder.group({
      name: ['', Validators.required],
      amount: ['', Validators.required, !isNaN, inParams],
      date: ['',Validators.required],
      type: ['', Validators.required]
    }));
  }
  

  addItem = (index) =>{

    let form = this.m.at(index).value;

    if (this.addTransactionForm.invalid) {
      this.notification = new errorObject(0,false,"Some fields not completed");
      return;
    }
    else{
      if(!form.date)
        form.date = Date.now();
      else
        form.date = new Date(form.date)
      let transaction = new Transaction(form.name,form.amount,form.date,this.user.Id)
      transaction.Type = form.type;
      this.transactionService.addTransaction(transaction).subscribe(
        data => {
          this.resetForm(index);
          this.notification = new errorObject(0,true,"Transaction Added Successfully")
        },
        err =>{
          console.log(err)
        }
      )


    }

  }


  deleteItem = (index) =>{
    this.m.removeAt(index);
  }

  resetForm(index){
    let form = this.m.at(index);
    form.reset();
  }

  clearNotification(){
    this.notification = null;
  }

  changePage(newPage: string){
    this.page = newPage;
  } 


}
