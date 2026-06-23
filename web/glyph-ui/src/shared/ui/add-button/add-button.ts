import { Component, input, output } from '@angular/core';

@Component({
  selector: 'add-button',
  imports: [],
  templateUrl: './add-button.html',
  styleUrl: './add-button.scss',
})
export class AddButton {
  disable = input(false);
  size = input<'sm' | 'md' | 'lg'>('md');
  type = input<'button' | 'submit' | 'reset'>('button');
  ariaLabel = input('Удалить');

  clicked = output<void>();

  onClick(): void{
    if (!this.disable()){
      this.clicked.emit();
    }
  }
}