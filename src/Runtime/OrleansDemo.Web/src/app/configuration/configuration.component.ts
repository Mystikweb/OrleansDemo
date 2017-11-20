import { Component, OnInit, ViewEncapsulation } from '@angular/core';

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

  constructor() { }

  ngOnInit() {
  }

}
