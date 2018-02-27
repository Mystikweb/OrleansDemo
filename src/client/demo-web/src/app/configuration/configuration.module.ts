import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ServicesModule } from '../services/services.module';

import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DeviceEditorComponent } from './device-list/device-editor.component';
import { SensorListComponent } from './sensor-list/sensor-list.component';
import { EventListComponent } from './event-list/event-list.component';

@NgModule({
  imports: [
    CommonModule,
    ConfigurationRoutingModule,
    SharedModule,
    ServicesModule
  ],
  declarations: [
    ConfigurationComponent,
    DeviceListComponent,
    DeviceEditorComponent,
    SensorListComponent,
    EventListComponent
  ],
  entryComponents: [
    DeviceEditorComponent
  ]
})
export class ConfigurationModule { }
