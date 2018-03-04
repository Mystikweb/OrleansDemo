import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { Observable } from 'rxjs/Observable';

import { DetailsHostItem, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceEditorComponent } from './device-editor.component';

import { DeviceConfig, DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent implements OnInit, AfterViewInit {
  devices$: Observable<DeviceConfig[]>;

  constructor(private deviceService: DeviceService,
    private detailsService: DetailsHostService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.detailsService.detailsOpen$.subscribe(opened => {
      if (opened === false) {
        this.loadData();
      }
    });
  }

  ngAfterViewInit() {
    this.loadData();
  }
  openEditor(device: DeviceConfig) {
    this.detailsService.openItem(new DetailsHostItem(DeviceEditorComponent, device === null ? new DeviceConfig() : device));
  }

  removeDevice(deviceId: string) {
    this.deviceService.remove(deviceId).subscribe(result => {
      this.snackBar.open('Removed successfully', 'Close', { duration: 1500 });
      this.loadData();
    });
  }

  private loadData() {
    this.devices$ = this.deviceService.getDeviceList();
  }
}
