import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';


@Injectable()
export class JwtTokenInterceptor implements HttpInterceptor{


    intercept(req: HttpRequest<any>,
        next: HttpHandler): Observable<any>{
            const jwtToken = localStorage.getItem("id_token");

            if(jwtToken){
                const cloned = req.clone({
                    headers: req.headers.set("Authorization", "Bearer " + jwtToken)
                })
                return next.handle(cloned);
            }
            else{
                return next.handle(req);
            }
        }



}