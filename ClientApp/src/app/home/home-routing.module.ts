
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { HomeComponent } from './home.component';
import { TemplateComponent } from '../template/template/template.component';


const routes: Routes = [
  { path: '',
    component: HomeComponent,
    children: [{
      path: '',
      pathMatch: 'full',
      redirectTo: 'dashboard'
    }, {
      path: 'dashboard',
      component: DashboardComponent
    },
    {
      path: 'user',
      component: TemplateComponent
    }
  ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
