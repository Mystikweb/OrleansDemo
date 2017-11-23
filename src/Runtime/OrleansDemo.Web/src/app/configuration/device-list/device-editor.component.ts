import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { DetailsItemComponent } from '../../shared/details-host/details-host.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceEditorComponent implements OnInit, DetailsItemComponent {
  data: any;

  constructor() { }

  ngOnInit() {
  }

}
