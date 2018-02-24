import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';
import { NavHeaderComponent } from './nav-header/nav-header.component';
import { DetailsHostService } from './details-host/details-host.service';
import { DetailsHostDirective } from './details-host/details-host.directive';
import { DetailsHostComponent } from './details-host/details-host.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  declarations: [
    NavHeaderComponent,
    DetailsHostDirective,
    DetailsHostComponent
  ],
  exports: [
    MaterialModule,
    NavHeaderComponent,
    DetailsHostComponent
  ],
  providers: [
    DetailsHostService
  ]
})
export class SharedModule { }
