import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';
import { NavHeaderComponent } from './nav-header/nav-header.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  declarations: [
    NavHeaderComponent
  ],
  exports: [
    MaterialModule,
    NavHeaderComponent
  ]
})
export class SharedModule { }
