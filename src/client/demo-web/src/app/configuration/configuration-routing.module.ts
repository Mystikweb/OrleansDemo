import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { SensorListComponent } from './sensor-list/sensor-list.component';
import { EventListComponent } from './event-list/event-list.component';

const routes: Routes = [{
  path: 'configuration',
  component: ConfigurationComponent,
  children: [
    { path: '', redirectTo: 'device', pathMatch: 'full' },
    { path: 'device', component: DeviceListComponent },
    { path: 'sensor', component: SensorListComponent },
    { path: 'event', component: EventListComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
