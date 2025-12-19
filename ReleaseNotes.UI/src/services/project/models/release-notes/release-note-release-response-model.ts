import { ReleaseNotesReleaseNoteEntryResponseModel } from "./release-notes-release-note-entry-response-model";

export interface ReleaseNoteReleaseResponseModel {
  id: string;
  version: string;
  createdOnUtc: string;
  publishedOn: Date | undefined;
  noteEntries: ReleaseNotesReleaseNoteEntryResponseModel[];
}
