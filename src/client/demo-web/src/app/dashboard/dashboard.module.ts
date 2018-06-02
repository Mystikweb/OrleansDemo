import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { ServicesModule } from '../services/services.module';

import { DashboardComponent } from './dashboard.component';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { DeviceListComponent } from './dashboard-home/device-list.component';
import { DeviceDetailsComponent } from './device-details/device-details.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    DashboardRoutingModule,
    ServicesModule
  ],
  declarations: [
    DashboardComponent,
    DashboardHomeComponent,
    DeviceListComponent,
    DeviceDetailsComponent
  ],
  entryComponents: [
    DeviceListComponent
  ]
})
export class DashboardModule { }
