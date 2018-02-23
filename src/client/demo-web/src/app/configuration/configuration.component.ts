import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css']
})
export class ConfigurationComponent implements OnInit {
  navLinks = [
    { label: 'Devices', path: 'device' },
    { label: 'Sensors', path: 'sensor'},
    { label: 'Events', path: 'event'}
  ];

  constructor() { }

  ngOnInit() {
  }

}
