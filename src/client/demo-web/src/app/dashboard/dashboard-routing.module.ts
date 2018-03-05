import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard.component';
import { DashboadHomeComponent } from './dashboad-home/dashboad-home.component';

const routes: Routes = [{
  path: 'dashboard',
  component: DashboardComponent,
  children: [
    { path: '', component: DashboadHomeComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
