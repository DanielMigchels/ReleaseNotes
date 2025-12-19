import { Component, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../services/users/user.service';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-update-password',
    imports: [GenericModalComponent, ReactiveFormsModule, NgIf],
    templateUrl: './update-password.component.html',
    styleUrl: './update-password.component.scss'
})
export class UpdatePasswordComponent extends GenericModalComponent {

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  passwordsDontMatch = false;
  error = false;

  formGroup = new FormGroup({
    currentPassword: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', [Validators.required]),
    repeatNewPassword: new FormControl('', [Validators.required]),
  });
  
  constructor(private userService: UserService) {
    super();
  }

  updatePassword() {
    this.passwordsDontMatch = false;
    this.error = false;
    if (!this.formGroup.valid) {
      return;
    }

    if (this.formGroup.value.newPassword !== this.formGroup.value.repeatNewPassword) {
       this.passwordsDontMatch = true;
       return;
    }

    this.userService.updatePassword(this.formGroup.value).subscribe({
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