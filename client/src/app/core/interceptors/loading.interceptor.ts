import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, identity } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { delay, finalize } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (req.url.includes('emailexists') || 
        req.method === 'POST' && req.url.includes('orders') || 
        req.method === 'DELETE'
        ) {
            return next.handle(req);
        }
        this.busyService.busy();
        return next.handle(req).pipe(
            //identity here like a null but it not null
          (environment.production ? identity :delay(500)),
            finalize(() => {
                this.busyService.idle();
            })
        );
    }
}