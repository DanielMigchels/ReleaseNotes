import { NgIf } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericModalComponent } from '../../../../components/generic-modal/generic-modal.component';
import { LoaderComponent } from '../../../../components/loader/loader.component';
import { BundleService } from '../../../../services/bundle/bundle.service';
import { AddProjectsToBundleRequestModel } from '../../../../services/bundle/models/add-projects-to-bundle-request-model';
import { BundleResponseModel } from '../../../../services/bundle/models/bundle-response-model';
import { ProjectResponseModel } from '../../../../services/project/models/project-response-model';
import { ProjectService } from '../../../../services/project/project.service';

@Component({
  selector: 'app-select-projects',
  imports: [GenericModalComponent, ReactiveFormsModule, LoaderComponent, NgIf],
  templateUrl: './select-projects.component.html',
  styleUrl: './select-projects.component.scss'
})
export class SelectProjectsComponent extends GenericModalComponent implements OnInit {

  private _bundle: BundleResponseModel | undefined;

  @Input() 
  set bundle(value: BundleResponseModel | undefined) {
    this._bundle = value;
    
    this.projectsFormArray.clear();

    this._bundle!.projects.forEach(projects => {
      this.projectsFormArray.push(new FormControl(projects.id));
    });
  }
  
  get bundle(): BundleResponseModel | undefined {
    return this._bundle;
  }
  
  projects: ProjectResponseModel[] = [];

  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;

  isSubmitting = false;

  formGroup = new FormGroup({
    projects: new FormArray([]),
  });
  
  get projectsFormArray(): FormArray {
    return this.formGroup.get('projects') as FormArray;
  }

  constructor(private _projectService: ProjectService, private _bundleService: BundleService) {
    super();
  }

  ngOnInit(): void {
    this.getProjects();
  }

  getProjects() {
    this._projectService.getProjects().subscribe({
      next: x => {
        this.projects = x.data;
      }
    });
  }

  addProjectsToBundle() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this._bundleService.addProjectsToBundle(this.bundle!.id, this.formGroup.value as AddProjectsToBundleRequestModel).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.modal.closeModal();
        this.isSubmitting = false;
      },
      error: () => {
        this.isSubmitting = false;
      }
    });
  }

  onProjectSelection(event: any) {
    const projectName = event.target.value;
    if (event.target.checked) {
      this.projectsFormArray.push(new FormControl(projectName));
    } else {
      const index = this.projectsFormArray.controls.findIndex(control => control.value === projectName);
      if (index !== -1) {
        this.projectsFormArray.removeAt(index);
      }
    }

    this.projectsFormArray.controls = this.projectsFormArray.controls.filter(control => control.value !== null);
  }

  isProjectSelected(projectName: string): boolean {
    return this.projectsFormArray.controls.some(control => control.value === projectName);
  }
}
