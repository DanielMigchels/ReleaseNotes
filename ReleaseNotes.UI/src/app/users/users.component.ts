import { NgIf } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgIconComponent } from '@ng-icons/core';
import { LoaderComponent } from '../../components/loader/loader.component';
import { CreateUserComponent } from "./create-user/create-user.component";
import { UserResponseModel } from '../../services/users/models/user-response-model';
import { UserService } from '../../services/users/user.service';
import { UpdatePasswordComponent } from "./update-password/update-password.component";
import { ChangeActivationComponent } from "./change-activation/change-activation.component";

@Component({
    selector: 'app-users',
    imports: [NgIconComponent, NgIf, LoaderComponent, CreateUserComponent, UpdatePasswordComponent, ChangeActivationComponent],
    templateUrl: './users.component.html',
    styleUrl: './users.component.scss'
})
export class UsersComponent implements OnInit {
  users: UserResponseModel[] | undefined;

  @ViewChild(ChangeActivationComponent) changeActivationModal!: ChangeActivationComponent;
  selectedUser: UserResponseModel | undefined;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService.getUsers().subscribe({
      next: users => this.users = users
    });
  }

  editUser($event: Event, user: any) {
    
  }

  changeActivation(event: Event, user: UserResponseModel) {
    event.stopPropagation();
    this.selectedUser = this.users?.find(x => x.id === user.id);
    this.changeActivationModal.modal.openModal(event);    
  }
}
