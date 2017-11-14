import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NavHeaderComponent implements OnInit {
  @Output() onToggled = new EventEmitter<boolean>();

  opened = false;

  constructor() { }

  ngOnInit() { }

  toggle() {
    this.opened = !this.opened;
    this.onToggled.emit(this.opened);
  }
}
