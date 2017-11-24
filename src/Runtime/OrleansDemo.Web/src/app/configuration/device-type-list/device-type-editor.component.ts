import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { DetailsItemComponent } from '../../shared/details-host/details-host.service';
import { DeviceType, DeviceTypeService } from '../../services/device-type.service';

@Component({
  selector: 'app-device-type-editor',
  templateUrl: './device-type-editor.component.html',
  styleUrls: ['./device-type-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeEditorComponent implements OnInit, DetailsItemComponent {
  data: any;

  constructor() { }

  ngOnInit() {
    console.log(this.data);
  }

}
