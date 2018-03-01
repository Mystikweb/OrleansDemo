import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { IDetailsHostComponent, DetailsHostService } from '../../shared/details-host/details-host.service';


import { DeviceConfig, DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css']
})
export class DeviceEditorComponent implements IDetailsHostComponent, OnInit {
  data: any;
  device: DeviceConfig;

  constructor(private detailsHost: DetailsHostService,
    private deviceService: DeviceService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.device = this.data as DeviceConfig;
    if (this.device.deviceId === undefined) {
      this.device.deviceId = null;
    }
  }

  save() {
    this.deviceService.save(this.device).subscribe(result => {
      this.snackBar.open('Saved successfully', 'Close', { duration: 1500 });
      this.detailsHost.closeItem();
    });
  }

  cancel() {
    this.detailsHost.closeItem();
  }
}
