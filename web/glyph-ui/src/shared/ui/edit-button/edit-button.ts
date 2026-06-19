import { Component, input, output } from '@angular/core';

@Component({
  selector: 'edit-button',
  imports: [],
  templateUrl: './edit-button.html',
  styleUrl: './edit-button.scss',
})
export class EditButton {
  disabled = input(false);
  size = input<'sm' | 'md' | 'lg'>('md');
  type = input<'button' | 'submit' | 'reset'>('button');
  ariaLabel = input('Изменить');

  clicked = output<void>();

  onClick(): void{
    if (!this.disabled()){
      this.clicked.emit();
    }
  }
}
