import { Component, OnInit } from '@angular/core';
import * as Chart from 'chart.js';

import {TransactionService} from '../services/transaction.service'
import {Analytics,SpendingDemographics,BiggestTransaction} from '../imports/server.models'
import { FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  canvas: any;
  ctx: any;

  analytics: Analytics;
  spendingDemographics: Array<SpendingDemographics> = [];
  chartSelection: string = "spending";

  form = new FormGroup({
    selection: new FormControl('spending')
  })

  chartData: Array<number> = [];
  chartLabels: Array<string> = [];
  chartType: string = 'pie';
  chartColors:Array<any> = [{
    hoverBorderColor: ['rgba(0, 0, 0, 0.1)', 'rgba(0, 0, 0, 0.1)', 'rgba(0, 0, 0, 0.1)', 'rgba(0, 0, 0, 0.1)', 'rgba(0, 0, 0, 0.1)'], 
    hoverBorderWidth: 0, 
    backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C", "#949FB1", "#4D5360"], 
    hoverBackgroundColor: ["#FF5A5E", "#5AD3D1", "#FFC870", "#A8B3C5","#616774"]
  }];

  constructor(private transactionService: TransactionService) 
  { 
    this.analytics = new Analytics();
    this.spendingDemographics;
  }

  ngOnInit(): void {
    this.getAnalytics();
  }
    
    ngAfterViewInit(){
      this.canvas = document.getElementById('pieChart');
      this.ctx = this.canvas.getContext('2d');
      const pieChart = new Chart(this.ctx, {
        type: 'pie',
        data: {
          labels: this.chartLabels,
          datasets: this.chartData,
          options: 
          {
            legend: 
            {
              display: false
            },
            responsive: true,
            display: true
          }
        }
        });
    }

  getAnalytics(){
    this.transactionService.getTransactionAnalytics().subscribe(
      data =>{
        console.log('Data', data)
        this.analytics.MostCommonTransactionType = data.mostCommonTransactionType;
        this.analytics.BiggestTransactionByAmount = new BiggestTransaction(data.biggestTransaction.type,data.biggestTransaction.amountSpent);
        this.analytics.TransactionCount = data.transactionCount;
        console.log("BiggestTrans", this.analytics.BiggestTransactionByAmount.Type)
        this.setSpendingDemographics(data.spendingDemographics);
        this.setChartData(this.spendingDemographics);
      },
      err =>{
        console.log(err)
      }
    )
  }

  setSpendingDemographics(data: Array<any>){
    for(var i = 0; i < data.length; i++){
      let demographic = new SpendingDemographics(data[i].type, data[i].numberOfTransactions, data[i].moneySpent)
      this.spendingDemographics.push(demographic);
      this.analytics.Demographics.push(demographic);
    }
  }

  setChartData(data: Array<SpendingDemographics>){
    
    this.resetChart();

    if(this.chartSelection === "spending")
    {
      this.setSpendingData(data);
    } else if(this.chartSelection === "transactions"){
      this.setTransactionData(data);
    }
    else {
      console.log('err');
    }

    console.log('chartData: ', this.chartData);
    
  }

  setSpendingData(data: Array<SpendingDemographics>){
    for(var i = 0; i < data.length; i++){

      let value = data[i].MoneySpent;
      this.chartData.push(value)
      this.chartLabels.push(data[i].Type)
    }
  }

  setTransactionData(data: Array<SpendingDemographics>){
    for(var i = 0; i < data.length; i++){

      let value = data[i].NumberOfTransaction;
      this.chartData.push(value)
      this.chartLabels.push(data[i].Type)
    }
  }

  changeSelection(){
    this.chartSelection = this.form.value.selection;
    console.log("chartSlection: ", this.chartSelection);
    this.setChartData(this.spendingDemographics);

  }

  resetChart(){
    this.chartData = [];
    this.chartLabels = [];
  }

}
