import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-searchbar',
  imports: [FormsModule],
  templateUrl: './searchbar.html',
  styleUrl: './searchbar.css',
})
export class Searchbar {
  searchText = '';

  @Output()
  search = new EventEmitter<string>();

  onSearch(): void {
    this.search.emit(this.searchText);
  }
}
 