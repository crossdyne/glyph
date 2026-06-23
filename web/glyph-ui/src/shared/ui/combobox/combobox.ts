import { Component, computed, effect, ElementRef, EventEmitter, HostListener, inject, Input, Output, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'combobox',
  imports: [],
  templateUrl: './combobox.html',
  styleUrl: './combobox.scss',
  standalone: true,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: Combobox,
      multi: true
    }
  ]
})
export class Combobox<T extends Record<string, any>> implements ControlValueAccessor {
  private elementRef = inject(ElementRef);
 
  @Input() items: T[] = [];
  @Input() placeholder = 'Выберите значение...';
  @Input() disabled = false;
  @Input() displayKey: keyof T = 'name' as keyof T;
  
  @Output() selectionChange = new EventEmitter<T | null>();

  isOpen = signal(false);
  searchText = signal('');
  selected = signal<T | null>(null);
  
  filteredItems = computed(() => {
    const search = this.searchText().toLowerCase();
    if (!search) return this.items;
    
    return this.items.filter(item => 
      String(item[this.displayKey]).toLowerCase().includes(search)
    );
  });

  private onChange: (value: T | null) => void = () => {};
  private onTouched: () => void = () => {};

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target) && this.isOpen()) {
      this.isOpen.set(false);
      this.onInputBlur();
    }
  }
  
  writeValue(value: T | null): void {
    this.selected.set(value);
    if (value) {
      this.searchText.set(String(value[this.displayKey]));
    } else {
      this.searchText.set('');
    }
  }

  registerOnChange(fn: (value: T | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchText.set(value);
    this.isOpen.set(true);
    
    if (!value) {
      this.selectItem(null);
    }
  }

  selectItem(item: T | null): void {
    this.selected.set(item);
    if (item) {
      this.searchText.set(String(item[this.displayKey]));
    } else {
      this.searchText.set('');
    }
    this.onChange(item);
    this.selectionChange.emit(item);
    this.isOpen.set(false);
  }

  toggleDropdown(): void {
    if (!this.disabled) {
      this.isOpen.update(v => !v);
    }
  }

  onInputBlur(): void {
    this.onTouched();
    const sel = this.selected();
    if (sel) {
      this.searchText.set(String(sel[this.displayKey]));
    }
    this.isOpen.set(false);
  }

  clear(): void {
    this.selected.set(null);
    this.searchText.set('');
    this.onChange(null);
    this.selectionChange.emit(null);
  }
}
