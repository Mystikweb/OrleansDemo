import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ConfigurationModule } from './configuration/configuration.module';

import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './material/material.module';
import { ServicesModule } from './services/services.module';

import { AppComponent } from './app.component';

import { HomeViewComponent } from './home-view/home-view.component';
import { NavHeaderComponent } from './nav-header/nav-header.component';

@NgModule({
  declarations: [
    AppComponent,
    NavHeaderComponent,
    HomeViewComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ConfigurationModule,
    AppRoutingModule,
    MaterialModule,
    ServicesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
