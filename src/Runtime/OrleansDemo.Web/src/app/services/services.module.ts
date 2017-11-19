import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { DeviceService } from './device.service';
import { DeviceTypeService } from './device-type.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [],
  providers: [
    DeviceTypeService,
    DeviceService
  ]
})
export class ServicesModule { }
