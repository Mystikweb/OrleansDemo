import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { ServicesModule } from '../services/services.module';
import { ConfigurationRoutingModule } from './configuration-routing.module';

import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DeviceEditorComponent } from './device-list/device-editor.component';
import { DeviceTypeListComponent } from './device-type-list/device-type-list.component';
import { DeviceTypeEditorComponent } from './device-type-list/device-type-editor.component';
import { ReadingTypeListComponent } from './reading-type-list/reading-type-list.component';
import { ReadingTypeEditorComponent } from './reading-type-list/reading-type-editor.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ConfigurationRoutingModule,
    MaterialModule,
    SharedModule,
    ServicesModule
  ],
  declarations: [
    ConfigurationComponent,
    DeviceListComponent,
    DeviceEditorComponent,
    DeviceTypeListComponent,
    DeviceTypeEditorComponent,
    ReadingTypeListComponent,
    ReadingTypeEditorComponent,
  ],
  bootstrap: [],
  entryComponents: [
    DeviceEditorComponent,
    DeviceTypeEditorComponent,
    ReadingTypeEditorComponent
  ],
  providers: []
})
export class ConfigurationModule { }
