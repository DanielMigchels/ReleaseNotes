import { Routes } from '@angular/router';
import { AuthLayoutComponent } from './layout/auth-layout/auth-layout.component';
import { LoginComponent } from './login/login.component';
import { authenticationGuard } from '../guards/authentication.guard';
import { BaseLayoutComponent } from './layout/base-layout/base-layout.component';
import { ProjectsComponent } from './projects/projects.component';
import { ReleaseNotesComponent } from './release-notes/release-notes.component';
import { UsersComponent } from './users/users.component';
import { BundleComponent } from './bundles/bundle/bundle.component';
import { BundlesComponent } from './bundles/bundles.component';

export const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    canActivate: [authenticationGuard],
    children: [
      { path: '', component: ProjectsComponent },
      { path: 'project/:id', component: ReleaseNotesComponent },
      { path: 'bundles', component: BundlesComponent },
      { path: 'bundles/:id', component: BundleComponent },
      { path: 'users', component: UsersComponent },
    ]
  },
  {
    path: 'login',
    component: AuthLayoutComponent,
    children: [
      { path: '', component: LoginComponent },
    ]
  },
  { path: '**', redirectTo: 'login', pathMatch: 'full' },
];
