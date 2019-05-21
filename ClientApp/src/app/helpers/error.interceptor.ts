import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '../services';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(catchError(err => {
			let error = '';
			if (err.status === 401) {
				error = 'Unauthorized';
                // auto logout if 401 response returned from api
				this.authenticationService.logout();
				if (location.pathname.toLowerCase() !== '/login')
					location.reload(true);
			}
			else
				error = err.error.message || err.statusText;

			
            return throwError(error);
        }))
    }
}