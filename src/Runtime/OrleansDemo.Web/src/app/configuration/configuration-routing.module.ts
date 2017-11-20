import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DeviceTypeListComponent } from './device-type-list/device-type-list.component';
import { ReadingTypeListComponent } from './reading-type-list/reading-type-list.component';

const routes: Routes = [{
  path: 'configuration',
  component: ConfigurationComponent,
  children: [
    { path: '', redirectTo: 'devices', pathMatch: 'full' },
    { path: 'devices', component: DeviceListComponent },
    { path: 'devicetypes', component: DeviceTypeListComponent },
    { path: 'readingtypes', component: ReadingTypeListComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
