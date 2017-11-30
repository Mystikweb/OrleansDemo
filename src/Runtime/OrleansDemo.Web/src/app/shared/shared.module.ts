import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FileSelectDirective, FileUploader } from 'ng2-file-upload';

import { MaterialModule } from '../material/material.module';

import { DetailsHostComponent } from './details-host/details-host.component';
import { DetailsHostDirective } from './details-host/details-host.directive';
import { DetailsHostService } from './details-host/details-host.service';
import { ImageFileUploadComponent } from './image-file-upload/image-file-upload.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    DetailsHostComponent,
    ImageFileUploadComponent
  ],
  declarations: [
    DetailsHostComponent,
    DetailsHostDirective,
    FileSelectDirective,
    ImageFileUploadComponent
  ],
  providers: [
    DetailsHostService,
    FileUploader
  ]
})
export class SharedModule { }
