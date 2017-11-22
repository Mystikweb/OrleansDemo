import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { ConfigurationDrawerService } from './configuration-drawer.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ConfigurationComponent implements OnInit {
  navLinks = [
    { label: 'Devices', path: 'devices' },
    { label: 'Device Types', path: 'devicetypes'},
    { label: 'Reading Types', path: 'readingtypes'}
  ];
  drawerOpen = false;

  constructor(private drawerService: ConfigurationDrawerService) { }

  ngOnInit() {
    this.drawerService.drawerOpen$.subscribe(opened => this.drawerOpen = opened);
  }

}
