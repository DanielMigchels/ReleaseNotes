import { Component, OnInit, ViewChild } from '@angular/core';
import { ProjectService } from '../../services/project/project.service';
import { ProjectResponseModel } from '../../services/project/models/project-response-model';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { LoaderComponent } from "../../components/loader/loader.component";
import { ReleaseResponseModel } from '../../services/release/models/release-response-model';
import { ReleaseService } from '../../services/release/release.service';
import { CreateProjectComponent } from './create-project/create-project.component';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { DeleteProjectComponent } from "./delete-project/delete-project.component";
import { EditProjectComponent } from "./edit-project/edit-project.component";
import { PaginatedList } from '../../services/pagination/paginated-list';

@Component({
    selector: 'app-projects',
    imports: [NgIf, LoaderComponent, CreateProjectComponent, NgIcon, DeleteProjectComponent, EditProjectComponent, DatePipe],
    templateUrl: './projects.component.html',
    styleUrl: './projects.component.scss'
})
export class ProjectsComponent implements OnInit {
  projects: PaginatedList<ProjectResponseModel> | undefined;
  releases: ReleaseResponseModel[] | undefined;

  selectedProject: ProjectResponseModel | undefined;
  @ViewChild(DeleteProjectComponent) deleteProjectModal!: DeleteProjectComponent;
  @ViewChild(EditProjectComponent) editProjectModal!: EditProjectComponent;

  constructor(private projectService: ProjectService, private releaseService: ReleaseService, private router: Router) { }

  pageSize = 2147483647;
  page = 0;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.projects = undefined;
    this.projectService.getProjects(this.pageSize, this.page).subscribe({
      next: x => this.projects = x
    });
    this.releaseService.getRecentReleases().subscribe({
      next: x => this.releases = x
    });
  }

  openProject(projectId: string) {
    this.router.navigate([`/project/${projectId}`]);
  }

  editProject(event: Event, project: ProjectResponseModel) {
    event.stopPropagation();
    this.selectedProject = project;
    this.editProjectModal.modal.openModal(event);
  }

  deleteProject(event: Event, project: ProjectResponseModel) {
    event.stopPropagation();
    this.selectedProject = project;
    this.deleteProjectModal.isChecked = false;
    this.deleteProjectModal.modal.openModal(event);
  }

  nextPage() {
    this.page++;
    this.fetchData();
  }

  previousPage() {
    this.page--;
    this.fetchData();
  }
}
