import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListUserComponent } from './components/list-user/list-user.component';
import { CommonModule } from '@angular/common';
import { CreateUserComponent } from './components/create-user/create-user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';
import { AdminListComponent } from './components/admin-list/admin-list.component';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { interceptorsInterceptor } from './intercepter/interceptors.interceptor';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HeaderComponent,
    SidebarComponent,
    LayoutComponent,
    DashboardComponent,
    ListUserComponent,
    CreateUserComponent,
    EditUserComponent,
    AdminListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [
    provideHttpClient(
      withInterceptors([interceptorsInterceptor])
    )
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
