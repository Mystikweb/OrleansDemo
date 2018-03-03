import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { DeviceService } from './device.service';
import { SensorService } from './sensor.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    DeviceService,
    SensorService
  ]
})
export class ServicesModule { }
