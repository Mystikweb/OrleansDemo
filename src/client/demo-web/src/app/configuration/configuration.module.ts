import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { SensorListComponent } from './sensor-list/sensor-list.component';
import { EventListComponent } from './event-list/event-list.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ConfigurationRoutingModule
  ],
  declarations: [
    ConfigurationComponent,
    DeviceListComponent,
    SensorListComponent,
    EventListComponent
  ]
})
export class ConfigurationModule { }
