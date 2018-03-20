import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { DeviceService } from './device.service';
import { SensorService } from './sensor.service';
import { GrainService } from './grain.service';
import { DashboardService } from './dashboard.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    DeviceService,
    SensorService,
    GrainService,
    DashboardService
  ]
})
export class ServicesModule { }
