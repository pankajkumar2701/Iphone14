import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpUrlEncodingCodec, HttpParams, HttpParameterCodec } from '@angular/common/http';
import { BehaviorSubject, EMPTY, Observable, of } from 'rxjs';
import { catchError, filter, finalize, switchMap, take, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TokenService } from '../token.service';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
    private language: string = 'en-us';
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    private refreshExpiredToken = false;

    constructor(
        private authService: AuthService,
        private route: Router,
        private tokenService: TokenService,
    ) { }

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        // show loader
        const expired = this.checkRefreshTokenExpied(request);
        if (expired === true) {
            // hide loader
            return EMPTY;
        } else {
            request = this.addAuthenticationToken(request);
            return this.refreshTokenAndRetry(request, next);
        }
    }
    private checkRefreshTokenExpied(request: HttpRequest<any>): boolean {
        if (this.route.url !== '/' && !request.url.includes('api/login') && !request.url.includes('api/refresh')) {
            if (this.tokenService.getTokenInfo().value && this.tokenService.isAuthTokenExpired()) {
                if (this.tokenService.isRefreshTokenExpired() === true) {
                    this.route.navigate(['Login']);
                    return true;
                } else {
                    this.refreshExpiredToken = true;
                    return false;
                }
            }
        } else {
            this.refreshExpiredToken = false;
        }
        return false;
    }
    private refreshTokenAndRetry(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
        if (this.refreshExpiredToken) {
            if (this.tokenService.tokenGettingRefreshed) {
                return this.refreshTokenSubject.pipe(
                    filter(result => result !== null),
                    take(1),
                    switchMap(() => this.handleRequest(request, next)
                    ));
            } else {
                this.tokenService.tokenGettingRefreshed = true;
                this.refreshTokenSubject.next(null);
                const refreshToken = this.tokenService.getRefreshToken() ?? '';
                return this.authService.refreshToken(refreshToken).pipe(
                    switchMap((data: any) => {
                        if (data) {
                            this.tokenService.setToken(data);
                            this.tokenService.tokenGettingRefreshed = false;
                            this.refreshTokenSubject.next(true);
                            this.refreshExpiredToken = false;
                        }
                        return this.handleRequest(request, next);
                    }),
                    finalize(() => this.tokenService.tokenGettingRefreshed = false)
                );
            }
        } else {
            return this.handleRequest(request, next);
        }
    }

    private addAuthenticationToken(request: HttpRequest<any>): HttpRequest<any> {
        const tokenInfo = this.tokenService.getTokenInfo();
        const params = new HttpParams({ encoder: new CustomEncoder(), fromString: request.params.toString() });
        const httpUrlEncoding = new HttpUrlEncodingCodec();

        if (tokenInfo.value) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${tokenInfo.value}`,
                    'Content-Type': 'application/json',
                    'langauage': this.language
                },
                params,
                url: httpUrlEncoding.encodeValue(request.url)
            });

            if (request.reportProgress) {
                request = request.clone({ headers: request.headers.delete('Content-Type', 'application/json') });
            }
        } else {
            request = request.clone({
                params,
                url: httpUrlEncoding.encodeValue(request.url),
            });
        }
        return request;
    }

    handleRequest(req: HttpRequest<any>, next: HttpHandler) {
        return next.handle(req).pipe(
            tap((event: any) => {
                if (event instanceof HttpResponse) {
                    // hide loader
                }
            }),
            catchError((error) => {
                if (error.status === 401) {
                    this.route.navigate(['/login']);
                }
                return of(error);
            }),
            finalize(() => {
                // hide loader
            })
        );
    }
}

export class CustomEncoder implements HttpParameterCodec {
    encodeKey(key: string): string {
        return encodeURIComponent(key);
    }

    encodeValue(value: string): string {
        return encodeURIComponent(value);
    }

    decodeKey(key: string): string {
        return decodeURIComponent(key);
    }

    decodeValue(value: string): string {
        return decodeURIComponent(value);
    }
}
