import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { DeviceType, DeviceTypeService } from '../../services/device-type.service';

import { DeviceTypeListComponent } from './device-type-list.component';

@Component({
  selector: 'app-device-type-dialog',
  templateUrl: './device-type-dialog.component.html',
  styleUrls: ['./device-type-dialog.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeDialogComponent implements OnInit {

  deviceType: DeviceType;

  constructor(
    private deviceTypeService: DeviceTypeService,
    public dialogRef: MatDialogRef<DeviceTypeListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeviceType) { }

  ngOnInit() {
    this.deviceType = this.data === null ? { id: null, name: null, active: false } : this.data;
  }

  save() {
    this.deviceTypeService.save(this.deviceType)
      .subscribe(result => {
          this.dialogRef.close(result);
      });
  }
}
