import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
    providedIn: 'root'
})
export class TokenService {
    tokenGettingRefreshed: boolean = false;

    setToken(token: any): void {
        localStorage.setItem('token', JSON.stringify(token));
    }

    getToken(): any {
        const token = localStorage.getItem('token');
        return token ? JSON.parse(token) : null;
    }

    logout(): void {
        localStorage.removeItem('token');
    }

    getTokenExpirationDate(token: string): Date {
        const decoded = jwtDecode(token);
        if (decoded['exp'] === undefined) {
            return new Date(0); // return default date if exp is undefined
        }
        const date = new Date(0);
        date.setUTCSeconds(decoded['exp']);
        return date;
    }

    isAuthTokenExpired(token?: string): boolean {
        if (!token) { token = this.getTokenInfo().value; }
        if (!token) { return true; }
        const date = this.getTokenExpirationDate(token);
        if (date === undefined) { return false; }
        return date.valueOf() <= new Date().valueOf();
    }

    isRefreshTokenExpired(token?: string): boolean {
        if (!token) { token = this.getRefreshToken()!; }
        if (!token) { return true; }
        const date = this.getTokenExpirationDate(token);
        if (date === undefined) { return true; }
        const retVal = date.valueOf() <= new Date().valueOf();
        return retVal;
    }

    getLoginToken(): string | null {
        const loginResponse = this.getToken();
        return loginResponse ? loginResponse.token : null;
    }

    getRefreshToken(): string | null {
        const loginResponse = this.getToken();
        return loginResponse ? loginResponse.refreshToken : null;
    }

    getTokenInfo(): any {
        const data = { value: '', isRemember: false };
        const token = this.getLoginToken();
        if (token) {
            data.value = token;
            data.isRemember = true;
        }
        return data;
    }
}
