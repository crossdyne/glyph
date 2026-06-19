import { Component, input, output } from '@angular/core';

@Component({
  selector: 'delete-button',
  imports: [],
  templateUrl: './delete-button.html',
  styleUrl: './delete-button.scss',
})
export class DeleteButton {
  disabled = input(false);
  size = input<'sm' | 'md' | 'lg'>('md');
  type = input<'button' | 'submit' | 'reset'>('button');
  ariaLabel = input('Удалить');

  clicked = output<void>();

  onClick(): void{
    if (!this.disabled()){
      this.clicked.emit();
    }
  }
}
