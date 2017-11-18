import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { DeviceApiService } from './device-api.service';
import { DeviceTypeService } from './device-type.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [],
  providers: [
    DeviceApiService,
    DeviceTypeService
  ]
})
export class ServicesModule { }
