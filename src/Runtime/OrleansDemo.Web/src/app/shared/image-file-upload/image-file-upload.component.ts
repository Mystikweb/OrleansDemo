import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';

import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-image-file-upload',
  templateUrl: './image-file-upload.component.html',
  styleUrls: ['./image-file-upload.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ImageFileUploadComponent implements OnInit {
  uploader: FileUploader | null;
  @Input() url: string;

  constructor() { }

  ngOnInit() {
    this.uploader = new FileUploader({ url: this.url });
  }

}
