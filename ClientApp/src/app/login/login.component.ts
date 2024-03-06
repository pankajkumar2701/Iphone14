import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginInfoPayload } from './login-info-payload';
import { Subject, first, takeUntil } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { TokenService } from '../angular-app-services/token.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  private formSubmitAttempt: boolean = false;
  protected destroy$ = new Subject();
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private tokenService: TokenService,
  ) {
    if (!this.tokenService.isAuthTokenExpired()) {
      this.navigateTo();
    }
  }

  ngOnInit() {
    this.form = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  isFieldInvalid(field: string) {
    return (
      (!this.form?.get(field)?.valid && this.form?.get(field)?.touched) ||
      (this.form?.get(field)?.untouched && this.formSubmitAttempt)
    );
  }

  onSubmit() {
    if (this.form.valid) {
      let loginDetail: LoginInfoPayload = {
        userName: this.form.value.userName,
        password: this.form.value.password
      };
      this.authService.login(loginDetail)
        .pipe(first(), takeUntil(this.destroy$))
        .subscribe({
          next: (token: any) => {
            this.tokenService.setToken(token);
            this.navigateTo();
          }
        });
    }
    this.formSubmitAttempt = true;
  }

  private navigateTo(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
    if (returnUrl) {
      this.router.navigate([returnUrl]);
    } else {
      this.router.navigate(['/dashboard']);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
