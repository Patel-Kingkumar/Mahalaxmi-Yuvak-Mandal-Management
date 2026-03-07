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
import { CreateDonationComponent } from './components/create-donation/create-donation.component';
import { ListDonationComponent } from './components/list-donation/list-donation.component';
import { ListMatchComponent } from './components/list-match/list-match.component';
import { CreateMatchComponent } from './components/create-match/create-match.component';
import { EditMatchComponent } from './components/edit-match/edit-match.component';
import { MatchDetailsComponent } from './components/match-details/match-details.component';

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
    AdminListComponent,
    CreateDonationComponent,
    ListDonationComponent,
    ListMatchComponent,
    CreateMatchComponent,
    EditMatchComponent,
    MatchDetailsComponent
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
