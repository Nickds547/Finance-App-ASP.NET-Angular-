import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

import {Transaction} from '../imports/server.models';
import {LINK,HEADERS} from '../imports/server.config';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) { }


  addTransaction = (transaction: Transaction): Observable<any> =>{
    let body = JSON.stringify(transaction);

    return this.http.post(LINK + "Transaction",body,{'headers' : HEADERS})
  }

  getTransactions = (): Observable<any> =>{
    return this.http.get(LINK + 'Transaction', {headers: HEADERS})
  }

  deleteTransactions = (transaction: Transaction): Observable<any> => {

    let deleteLink = LINK + 'Transaction/' + transaction.TransactionId;

    return this.http.delete(deleteLink);
  }

  updateTransaction = (transaction: Transaction):Observable<any> =>{
    let body = JSON.stringify(transaction);

    return this.http.put(LINK + 'Transaction/' + transaction.TransactionId, body, {'headers' : HEADERS})
  }
}
