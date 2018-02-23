import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationHomeComponent } from './configuration-home/configuration-home.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ConfigurationRoutingModule
  ],
  declarations: [
    ConfigurationHomeComponent
  ]
})
export class ConfigurationModule { }
