
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { canActivateTeam } from './angular-app-services/permissions.service';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: '',
    canActivateChild: [canActivateTeam],
    loadChildren: () =>
      import('./home/home.module').then((m) => m.HomeModule)/*,
    canLoad: [AuthGuard],*/
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
