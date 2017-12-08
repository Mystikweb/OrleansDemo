import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../material/material.module';

import { DetailsHostComponent } from './details-host/details-host.component';
import { DetailsHostDirective } from './details-host/details-host.directive';
import { DetailsHostService } from './details-host/details-host.service';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    DetailsHostComponent
  ],
  declarations: [
    DetailsHostComponent,
    DetailsHostDirective,
  ],
  providers: [
    DetailsHostService
  ]
})
export class SharedModule { }
