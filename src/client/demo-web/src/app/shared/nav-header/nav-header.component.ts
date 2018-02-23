import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.css']
})
export class NavHeaderComponent implements OnInit {
  @Output() toggleNavMenu = new EventEmitter<boolean>();

  opened = false;

  constructor() { }

  ngOnInit() {
  }

  toggle() {
    this.opened = !this.opened;
    this.toggleNavMenu.emit(this.opened);
  }
}
