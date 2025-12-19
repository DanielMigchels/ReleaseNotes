import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReleaseResponseModel } from './models/release-response-model';
import { CreateReleaseRequestModel } from './models/create-release-request-model';
import { EditReleaseRequestModel } from './edit-release-request-model';

@Injectable({
  providedIn: 'root'
})
export class ReleaseService {

  private apiUrl = '/api/release';

  constructor(private http: HttpClient) { }

  getRecentReleases(): Observable<ReleaseResponseModel[]> {
    return this.http.get<ReleaseResponseModel[]>(`${this.apiUrl}/recent`);
  }

  publishRelease(releaseId: string) {
    return this.http.post(`${this.apiUrl}/${releaseId}/publish`, null);
  }

  unpublishRelease(releaseId: string) {
    return this.http.post(`${this.apiUrl}/${releaseId}/unpublish`, null);
  }

  createRelease(projectId: string, createReleaseRequestModel: CreateReleaseRequestModel) {
    return this.http.post(`${this.apiUrl}/${projectId}`, createReleaseRequestModel);
  }

  editRelease(releaseId: string, editReleaseRequestModel: EditReleaseRequestModel) {
    return this.http.put(`${this.apiUrl}/${releaseId}`, editReleaseRequestModel);
  }

  deleteRelease(releaseId: string) {
    return this.http.delete(`${this.apiUrl}/${releaseId}`);
  }
}
