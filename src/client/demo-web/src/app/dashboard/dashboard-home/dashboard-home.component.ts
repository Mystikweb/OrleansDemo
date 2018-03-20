import { AfterViewInit, Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import { DeviceSummary, SensorSummary, DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements OnInit, AfterViewInit {
  dashboard$: Observable<DeviceSummary[]>;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.loadData();
  }

  showDevices() {

  }

  private loadData() {
    this.dashboard$ = this.dashboardService.getDashboardSummary();
  }
}
