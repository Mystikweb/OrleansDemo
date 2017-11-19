import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';
import { ServicesModule } from '../services/services.module';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationComponent } from './configuration.component';
import { DeviceTypeListComponent } from './device-type-list/device-type-list.component';
import { DeviceListComponent } from './device-list/device-list.component';

@NgModule({
  imports: [
    CommonModule,
    ConfigurationRoutingModule,
    MaterialModule,
    ServicesModule
  ],
  declarations: [
    ConfigurationComponent,
    DeviceTypeListComponent,
    DeviceListComponent
  ]
})
export class ConfigurationModule { }
