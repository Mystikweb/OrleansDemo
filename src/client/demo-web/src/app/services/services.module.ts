import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { DeviceService } from './device.service';
import { SensorService } from './sensor.service';
import { GrainService } from './grain.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    DeviceService,
    SensorService,
    GrainService
  ]
})
export class ServicesModule { }
