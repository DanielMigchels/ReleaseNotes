import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateNoteRequestModel } from './models/create-note-request-model';
import { EditNoteRequestModel } from './models/edit-note-request-model';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  private apiUrl = '/api/note';

  constructor(private http: HttpClient) { }

  createNote(releaseId: string, createNoteRequestModel: CreateNoteRequestModel) {
    return this.http.post(`${this.apiUrl}/${releaseId}`, createNoteRequestModel);
  }

  moveNote(noteId: string, releaseId: string) {
    return this.http.put(`${this.apiUrl}/${noteId}/${releaseId}`, null);
  }

  editNote(noteId: string, editNoteRequestModel: EditNoteRequestModel) {
    return this.http.put(`${this.apiUrl}/${noteId}`, editNoteRequestModel);
  }

  deleteNote(noteId: string) {
    return this.http.delete(`${this.apiUrl}/${noteId}`);
  }
}
