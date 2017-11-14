import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MaterialModule } from './material/material.module';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

import { HomeViewComponent } from './home-view/home-view.component';
import { NavHeaderComponent } from './nav-header/nav-header.component';
import { ConfigurationViewComponent } from './configuration-view/configuration-view.component';

@NgModule({
  declarations: [
    AppComponent,
    NavHeaderComponent,
    HomeViewComponent,
    ConfigurationViewComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
