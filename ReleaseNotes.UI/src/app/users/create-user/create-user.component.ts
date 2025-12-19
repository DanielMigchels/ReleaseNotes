import { Component, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../services/users/user.service';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-create-user',
    imports: [GenericModalComponent, ReactiveFormsModule, NgIf],
    templateUrl: './create-user.component.html',
    styleUrl: './create-user.component.scss'
})
export class CreateUserComponent extends GenericModalComponent {
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  
  formGroup = new FormGroup({
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });
  
  passwordsDontMatch = false;
  error = false;

  constructor(private userService: UserService) {
    super();
  }

  createUser() {
    this.passwordsDontMatch = false;
    this.error = false;
    if (!this.formGroup.valid) {
      return;
    }

    if (this.formGroup.value.password !== this.formGroup.value.confirmPassword) {
       this.passwordsDontMatch = true;
       return;
    }

    this.userService.createUser(this.formGroup.value).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
      },
      error: () => {
        this.error = true;
      }
    });
  }
}
