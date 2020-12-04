import { Component, OnInit } from '@angular/core';
import * as Chart from 'chart.js';

import {TransactionService} from '../services/transaction.service'
import {Analytics,TypesPurchased} from '../imports/server.models'


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  canvas: any;
  ctx: any;

  analytics: Analytics;
  typesPurchased: Map<string, TypesPurchased>;

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
    this.typesPurchased = new Map();
  }

  ngOnInit(): void {
    this.getAnalytics();
  }
    
    ngAfterViewInit(){
      this.canvas = document.getElementById('pieChart');
      //this.ctx = this.canvas.getContext('2d');
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
        this.analytics.MostCommonTransactionType = data.mostCommonTransactionType;
        this.analytics.TransactionCount = data.transactionCount;
        this.setChartData(data.purchasedTypes)
        this.setTypesPurchased(data.purchasedTypes);
      },
      err =>{
        console.log(err)
      }
    )
  }

  setTypesPurchased(data: Array<any>){
    for(var i = 0; i < data.length; i++){
      let typesPurchased = new TypesPurchased(data[i].type, data[i].amountPurchased)
      this.typesPurchased.set(data[i].type, typesPurchased);
      this.analytics.PurchasedTypes.push(typesPurchased);
      
    }
  }

  setChartData(data: Array<any>){
    for(var i = 0; i < data.length; i++){

      let value = data[i].amountPurchased;
      this.chartData.push(value)
      this.chartLabels.push(data[i].type)
    }

    console.log(this.analytics.PurchasedTypes)
  }

}
