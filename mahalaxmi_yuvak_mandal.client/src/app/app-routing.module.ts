import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ListUserComponent } from './components/list-user/list-user.component';
import { CreateUserComponent } from './components/create-user/create-user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';
import { AdminListComponent } from './components/admin-list/admin-list.component';
import { authGuard } from './guards/guards/auth.guard';
import { CreateDonationComponent } from './components/create-donation/create-donation.component';
import { ListDonationComponent } from './components/list-donation/list-donation.component';
import { MatchDetailsComponent } from './components/match-details/match-details.component';
import { EditMatchComponent } from './components/edit-match/edit-match.component';
import { CreateMatchComponent } from './components/create-match/create-match.component';
import { ListMatchComponent } from './components/list-match/list-match.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],   // 👈 guard applied here
    children: [
      { path: 'dashboard', component: DashboardComponent },

      {
        path: 'create-user',
        component: CreateUserComponent,
        canActivate: [authGuard],
        data: { roles: ['admin'] }
      },

      {
        path: 'edit-user/:id',
        component: EditUserComponent,
        canActivate: [authGuard],
        data: { roles: ['admin'] }
      },
      {
        path: 'create-donation',
        component: CreateDonationComponent,
        canActivate: [authGuard],
        data: { roles: ['admin'] }
      },
      { path: 'list-users', component: ListUserComponent },
      { path: 'list-admins', component: AdminListComponent },
      { path: 'list-donation', component: ListDonationComponent },

      // match moduel
      { path: 'list-match', component: ListMatchComponent },
      { path: 'create-match', component: CreateMatchComponent },
      { path: 'edit-match/:id', component: EditMatchComponent },
      { path: 'match-details/:id', component: MatchDetailsComponent },
    ]
  },

  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
