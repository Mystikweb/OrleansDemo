import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { IDetailsHostComponent, DetailsHostService } from '../../shared/details-host/details-host.service';
import { SensorConfig, SensorService } from '../../services/sensor.service';

@Component({
  selector: 'app-sensor-editor',
  templateUrl: './sensor-editor.component.html',
  styleUrls: ['./sensor-editor.component.css']
})
export class SensorEditorComponent implements IDetailsHostComponent, OnInit {
  data: any;
  sensor: SensorConfig;

  constructor(private detailsHost: DetailsHostService,
    private sensorService: SensorService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.sensor = this.data as SensorConfig;
    if (this.sensor.sensorId === undefined) {
      this.sensor.sensorId = null;
    }
  }

  save() {
    this.sensorService.save(this.sensor).subscribe(result => {
      this.snackBar.open('Saved successfully', 'Close', { duration: 1500 });
      this.detailsHost.closeItem();
    });
  }

  cancel() {
    this.detailsHost.closeItem();
  }
}
