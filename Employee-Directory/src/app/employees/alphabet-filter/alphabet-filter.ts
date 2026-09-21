import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-alphabet-filter',
  imports: [CommonModule],
  templateUrl: './alphabet-filter.html',
  styleUrl: './alphabet-filter.css',
})
export class AlphabetFilterComponent {

  alphabet: string[] = Array.from({ length: 26 }, (_, i) => String.fromCharCode(65 + i));

  activeLetter: string | null = null;

  @Output() letterSelected = new EventEmitter<string | null>();

  selectLetter(letter: string): void {
    if (this.activeLetter === letter) {
      this.activeLetter = null;
    } else {
      this.activeLetter = letter;
    }
    this.letterSelected.emit(this.activeLetter);
  }

  resetFilter(): void {
    this.activeLetter = null;
    this.letterSelected.emit(null);
  }
}
