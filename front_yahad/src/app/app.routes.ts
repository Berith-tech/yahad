import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ResetPasswordComponent } from './reset-password/components/reset-password-smart/reset-password.component';

export const routes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'reset-password', component: ResetPasswordComponent },
];
