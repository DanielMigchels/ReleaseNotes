import { NgIf } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { GenericModalComponent } from "../../../components/generic-modal/generic-modal.component";
import { ProjectService } from '../../../services/project/project.service';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { AddProjectRequestModel } from '../../../services/project/models/add-project-request-model';
import { Router } from '@angular/router';
import { LoaderComponent } from "../../../components/loader/loader.component";

@Component({
    selector: 'app-create-project',
    imports: [NgIf, GenericModalComponent, ReactiveFormsModule, LoaderComponent],
    templateUrl: './create-project.component.html',
    styleUrl: './create-project.component.scss'
})
export class CreateProjectComponent extends GenericModalComponent {
  
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  formGroup = new FormGroup({
    name: new FormControl('', [Validators.required])
  });

  isSubmitting: boolean = false;

  constructor(private projectService: ProjectService, private router: Router) { super(); }

  createProject() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.projectService.addProject(this.formGroup.value as AddProjectRequestModel).subscribe({
      next: x => {
        this.modalClosed.emit();
        this.formGroup.reset();
        this.modal.closeModal();
        this.router.navigate([`/project/${x.id}`]);
        this.isSubmitting = false;
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }
}
