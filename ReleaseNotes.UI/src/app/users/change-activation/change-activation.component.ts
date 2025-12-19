import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { UserResponseModel } from '../../../services/users/models/user-response-model';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../services/users/user.service';

@Component({
    selector: 'app-change-activation',
    imports: [GenericModalComponent, ReactiveFormsModule],
    templateUrl: './change-activation.component.html',
    styleUrl: './change-activation.component.scss'
})
export class ChangeActivationComponent extends GenericModalComponent {
  private _user: UserResponseModel | undefined;

  @Input()
  set user(value: UserResponseModel | undefined) {
    this._user = value;
    if (this._user) {
      this.formGroup.patchValue({
        activated: this._user.activated,
      });
    }
  }
  get user(): UserResponseModel | undefined {
    return this._user;
  }

  formGroup = new FormGroup({
    activated: new FormControl(false, [Validators.required]),
  });

  constructor(private userService: UserService) {
    super();
  }

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  changeAccess() {
    if (!this.formGroup.valid) {
      return;
    }

    this.userService.changeAccess(this.user!.id, this.formGroup.value).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
      }
    });
  }
}
