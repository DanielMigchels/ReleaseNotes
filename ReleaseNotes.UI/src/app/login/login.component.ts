import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginRequestModel } from '../../services/authentication/models/login-request-model';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    imports: [ReactiveFormsModule, NgIf],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent {

  rejected = false;

  formGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });
  
  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  login() {
    this.rejected = false;

    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.authenticationService.login(this.formGroup.value as LoginRequestModel).subscribe({
      next: response => {
        if (response.success) {
          this.authenticationService.setJwt(response.jwt);
          this.router.navigate(['/']);
        }
        else {
          this.rejected = true;
        }
      },
      error: error => {
        this.rejected = true;
      }
    }
    );
  }
}
