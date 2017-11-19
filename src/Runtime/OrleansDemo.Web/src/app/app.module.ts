import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HomeModule } from './home/home.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { ConfigurationModule } from './configuration/configuration.module';

import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './material/material.module';
import { ServicesModule } from './services/services.module';

import { AppComponent } from './app.component';

import { NavHeaderComponent } from './nav-header/nav-header.component';

@NgModule({
  declarations: [
    AppComponent,
    NavHeaderComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    HomeModule,
    DashboardModule,
    ConfigurationModule,
    AppRoutingModule,
    ServicesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
