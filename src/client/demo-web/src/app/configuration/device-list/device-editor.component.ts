import { Component, OnInit } from '@angular/core';

import { IDetailsHostComponent, DetailsHostService } from '../../shared/details-host/details-host.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css']
})
export class DeviceEditorComponent implements IDetailsHostComponent, OnInit {
  data: any;

  constructor() { }

  ngOnInit() {
  }

}
