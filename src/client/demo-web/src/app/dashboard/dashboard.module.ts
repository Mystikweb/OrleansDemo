import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { DashboadHomeComponent } from './dashboad-home/dashboad-home.component';
import { DeviceDetailsComponent } from './device-details/device-details.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    DashboardRoutingModule
  ],
  declarations: [
    DashboardComponent,
    DashboadHomeComponent,
    DeviceDetailsComponent
  ]
})
export class DashboardModule { }
