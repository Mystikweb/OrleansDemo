import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { DetailsItemComponent, DetailsHostService } from '../../shared/details-host/details-host.service';

import { Device, Reading, DeviceService } from '../../services/device.service';
import { DeviceType, DeviceReadingType, DeviceTypeService } from '../../services/device-type.service';

@Component({
  selector: 'app-device-editor',
  templateUrl: './device-editor.component.html',
  styleUrls: ['./device-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceEditorComponent implements OnInit, DetailsItemComponent {
  data: any;
  device: Device;
  deviceTypes: DeviceType[];

  constructor(private detailsHost: DetailsHostService,
    private deviceService: DeviceService,
    private deviceTypeService: DeviceTypeService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.device = this.data as Device;
    this.deviceTypeService.getList()
      .subscribe(results => {
        this.deviceTypes = results.filter(t => t.active === true);
        this.typeSelected();
      });
  }

  typeSelected() {
    if (this.device.deviceTypeId !== null) {
      const selectedType = this.deviceTypes.find(t => t.id === this.device.deviceTypeId);
      if (selectedType) {
        this.setupReadings(selectedType.readingTypes);
      }
    }
  }

  save() {
    if (this.device.id === undefined) {
      this.deviceService.save(this.device).subscribe(result => {
        this.snackBar.open('Saved successfully', 'Close', { duration: 1500 });
        this.detailsHost.closeItem();
      });
    } else {
      this.deviceService.update(this.device).subscribe(result => {
        this.snackBar.open('Update successfully', 'Close', { duration: 1500 });
        this.detailsHost.closeItem();
      });
    }
  }

  cancel() {
    this.detailsHost.closeItem();
  }

  private setupReadings(readingTypes: DeviceReadingType[]) {
    readingTypes.forEach(t => {
      const existing = this.device.readings.find(r => r.readingTypeId === t.readingTypeId);
      if (!existing) {
        const reading = new Reading();
        reading.readingTypeId = t.readingTypeId;
        reading.readingType = t.readingType;
        reading.readingUom = t.readingTypeUom;
        reading.readingDataType = t.readingTypeDataType;
        reading.enabled = false;

        this.device.readings.push(reading);
      }
    });
  }
}
