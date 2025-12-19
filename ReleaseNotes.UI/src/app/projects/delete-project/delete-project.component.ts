import { Component, Input, ViewChild } from '@angular/core';
import { GenericModalComponent } from '../../../components/generic-modal/generic-modal.component';
import { ProjectResponseModel } from '../../../services/project/models/project-response-model';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectService } from '../../../services/project/project.service';

@Component({
    selector: 'app-delete-project',
    imports: [GenericModalComponent, NgIf, FormsModule],
    templateUrl: './delete-project.component.html',
    styleUrl: './delete-project.component.scss'
})
export class DeleteProjectComponent extends GenericModalComponent {
  @Input() project: ProjectResponseModel | undefined;
  @ViewChild(GenericModalComponent) modal!: GenericModalComponent;
  isChecked: boolean = false;

  constructor(private projectService: ProjectService) {
    super();
  }

  deleteProject() {
    this.projectService.deleteProject(this.project!.id).subscribe({
      next: () => {
        this.modalClosed.emit();
        this.modal.closeModal();
      }
    });
  }
}
