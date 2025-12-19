import { NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
    selector: 'app-generic-modal',
    imports: [NgIf, NgIcon],
    templateUrl: './generic-modal.component.html',
    styleUrl: './generic-modal.component.scss'
})
export class GenericModalComponent {

  showModal = false;
  @Output() modalClosed = new EventEmitter();
  @Input() title: string = '';

  constructor() { }

  openModal(event: Event) {
    event.stopPropagation();
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }
}
