import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfigurationHomeComponent } from './configuration-home/configuration-home.component';

const routes: Routes = [{
  path: 'configuration',
  component: ConfigurationHomeComponent,
  children: []
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
