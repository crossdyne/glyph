import { Component, ElementRef, input, output, signal, ViewChild } from "@angular/core";

@Component({
    selector: 'file-uploader',
    templateUrl: './file-uploader.component.html',
    styleUrls: ['./file-uploader.component.scss'],
    standalone: true
})
export class FileUploaderComponent {
  accept = input('');
  multiple = input(false);
  maxSizeMB = input(10);

  filesSelected = output<File[]>();
  uploadError = output<string>();

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  isDragOver = signal(false);

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(true);
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(false);
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(false);

    if (event.dataTransfer?.files) {
      this.handleFiles(event.dataTransfer.files);
    }
  }

  openFilePicker() {
    this.fileInput.nativeElement.click();
  }

  onFileInputChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.handleFiles(input.files);
    }
  }

  private handleFiles(fileList: FileList) {
    const filesArray = Array.from(fileList);
    const maxBytes = this.maxSizeMB() * 1024 * 1024;

    const file = filesArray[0];
    if (!file) return;

    if (file.size > maxBytes) {
      this.uploadError.emit(`Файл "${file.name}" превышает максимальный размер ${this.maxSizeMB()} МБ`);
      return;
    }

    this.filesSelected.emit([file]);
    this.fileInput.nativeElement.value = '';
  }
}