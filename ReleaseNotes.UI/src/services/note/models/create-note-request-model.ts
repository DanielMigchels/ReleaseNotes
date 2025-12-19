import { NoteEntryType } from "../../../enums/note-entry-type";

export interface CreateNoteRequestModel {
  text: string;
  url: string | undefined;
  type: NoteEntryType | undefined;
}
