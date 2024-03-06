import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/angular-app-services/token.service';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.scss'
})
export class TopBarComponent {
  constructor(
    private router: Router,
    private tokenService: TokenService,
  ) {
  }

  logout(): void {
    this.tokenService.logout();
    this.router.navigate(['/']);
  }
}
