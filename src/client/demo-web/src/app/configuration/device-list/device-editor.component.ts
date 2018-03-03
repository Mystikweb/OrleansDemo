import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Observable } from 'rxjs/Observable';

import { IDetailsHostComponent, DetailsHostService } from '../../shared/details-host/details-host.service';

import { DeviceConfig, DeveiceSensorConfig, DeviceService } from '../../services/device.service';
import { SensorConfig, SensorService } from '../../services/sensor.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css']
})
export class DeviceEditorComponent implements IDetailsHostComponent, OnInit {
  data: any;
  device: DeviceConfig;
  sensors: SensorConfig[];

  constructor(private detailsHost: DetailsHostService,
    private deviceService: DeviceService,
    private sensorService: SensorService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.device = this.data as DeviceConfig;
    if (this.device.deviceId === undefined) {
      this.device.deviceId = null;
    }

    this.sensorService.getSensorList().subscribe(results => {
      console.log(results);
    });
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
