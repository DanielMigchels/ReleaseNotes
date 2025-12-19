import { NoteEntryType } from "../../../../enums/note-entry-type";

export interface ReleaseNotesReleaseNoteEntryResponseModel {
  id: string;
  text: string;
  url: string;
  type: NoteEntryType;
  createdOnUtc: Date;
  releasePublishedOn: Date;
}
 