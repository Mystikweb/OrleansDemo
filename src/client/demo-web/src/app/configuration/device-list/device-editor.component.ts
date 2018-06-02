import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Observable } from 'rxjs';

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

    // this.sensorService.getSensorList().subscribe(results => {
    //   results.forEach(element => {
    //     if (this.device.sensors.findIndex(s => s.sensorId === element.sensorId) === -1) {
    //       const devSensor = new DeveiceSensorConfig();
    //       devSensor.deviceSensorId = null;
    //       devSensor.sensorId = element.sensorId;
    //       devSensor.name = element.name;
    //       devSensor.isEnabled = false;

    //       this.device.sensors.push(devSensor);
    //     }
    //   });
    // });
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
