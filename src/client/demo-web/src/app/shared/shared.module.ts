import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';
import { NavHeaderComponent } from './nav-header/nav-header.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  declarations: [
    NavHeaderComponent,
    NavMenuComponent
  ],
  exports: [
    MaterialModule,
    NavHeaderComponent,
    NavMenuComponent
  ]
})
export class SharedModule { }
