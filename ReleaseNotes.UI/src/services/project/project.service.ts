import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddProjectRequestModel } from './models/add-project-request-model';
import { ProjectResponseModel } from './models/project-response-model';
import { ReleaseNotesResponseModel } from './models/release-notes/release-notes-response-model';
import { AddProjectResponseModel } from './models/add-project-response-model';
import { EditProjectRequestModel } from './models/edit-project-request-model';
import { PaginatedList } from '../pagination/paginated-list';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = '/api/project';

  constructor(private http: HttpClient) { }

  getProjects(pageSize = 2147483647, page = 0): Observable<PaginatedList<ProjectResponseModel>> {
    return this.http.get<PaginatedList<ProjectResponseModel>>(`${this.apiUrl}?pageSize=${pageSize}&page=${page}`);
  }

  getReleaseNotes(projectId: string): Observable<ReleaseNotesResponseModel> {
    return this.http.get<ReleaseNotesResponseModel>(`${this.apiUrl}/${projectId}`);
  }

  downloadPdf(projectId: string, includeUnpublished: boolean): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${projectId}/pdf?includeUnpublished=${includeUnpublished}`, { responseType: 'blob' });
  }

  addProject(addProjectRequestModel: AddProjectRequestModel): Observable<AddProjectResponseModel> {
    return this.http.post<AddProjectResponseModel>(`${this.apiUrl}`, addProjectRequestModel);
  }

  editProject(projectId: string, editProjectRequestModel: EditProjectRequestModel) {
    return this.http.put(`${this.apiUrl}/${projectId}`, editProjectRequestModel);
  }

  deleteProject(projectId: string) {
    return this.http.delete(`${this.apiUrl}/${projectId}`);
  }
}
