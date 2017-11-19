import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NavHeaderComponent implements OnInit {
  @Output() onToggled = new EventEmitter<boolean>();

  opened = false;

  constructor(private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        this.opened = false;
      }
    });
   }

  toggle() {
    this.opened = !this.opened;
    this.onToggled.emit(this.opened);
  }
}
