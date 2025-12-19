import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { NgIconComponent } from '@ng-icons/core';

@Component({
    selector: 'app-base-layout',
    imports: [RouterOutlet, NgIconComponent],
    templateUrl: './base-layout.component.html',
    styleUrl: './base-layout.component.scss'
})
export class BaseLayoutComponent {
  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  logout() {
    this.authenticationService.logout();
  }

  navigateBundle() { 
    this.router.navigate(['/bundles']);
  }

  navigateUsers() { 
    this.router.navigate(['/users']);
  }

  navigateHome() {
    this.router.navigate(['/']);
  }
}
