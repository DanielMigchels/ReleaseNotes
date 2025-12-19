import { NgIf, CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { LoaderComponent } from '../../../components/loader/loader.component';
import { ProjectService } from '../../../services/project/project.service';
import { ProjectResponseModel } from '../../../services/project/models/project-response-model';
import { EditProjectRequestModel } from '../../../services/project/models/edit-project-request-model';

@Component({
    selector: 'app-edit-project',
    imports: [NgIf, GenericModalComponent, ReactiveFormsModule, CommonModule, LoaderComponent],
    templateUrl: './edit-project.component.html',
    styleUrl: './edit-project.component.scss'
})
export class EditProjectComponent extends GenericModalComponent{
  private _project: ProjectResponseModel | undefined;

  @Input() 
  set project(value: ProjectResponseModel | undefined) {
    this._project = value;
    if (this._project) {
      this.formGroup.patchValue({
        name: this._project.name,
      });
    }
  }
  get project(): ProjectResponseModel | undefined {
    return this._project;
  }
  
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isSubmitting: boolean = false;

  formGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
  });

  constructor(private projectService: ProjectService) {
    super();
  }

  editProject() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.projectService.editProject(this.project!.id, this.formGroup.value as EditProjectRequestModel).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.modalClosed.emit();
        this.modal.closeModal();
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }
}
