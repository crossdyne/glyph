import { Component, effect, input, OnInit, output, signal } from "@angular/core";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";

@Component({
    selector: 'svg-form',
    templateUrl: './svg-form.component.html',
    styleUrls: ['./svg-form.component.scss'],
    standalone: true,
    imports: [ReactiveFormsModule],
})
export class SvgFormComponent implements OnInit {
  
  asset = input<AssetUrlResponse | null>(null);
  selectedFile = input<File | null>(null);

  created = output<string>(); 
  updated = output<UpdateAssetRequest>();
  cancelled = output<void>();

  form!: FormGroup;
  previewUrl = signal<string | null>(null);
  saving = signal(false);
  uploadError = signal<string | null>(null);

  constructor() {
    effect(() => {
      const assetData = this.asset();
      const file = this.selectedFile();
      
      if (!assetData && !file) {
        this.previewUrl.set(null);
        this.form?.get('assetName')?.setValue('', { emitEvent: false });
        this.uploadError.set(null);
        return;
      }
      
      if (file) {
        this.previewUrl.set(URL.createObjectURL(file));
      } else if (assetData) {
        this.previewUrl.set(assetData.url);
        this.form?.get('assetName')?.setValue(assetData.assetName, { emitEvent: false });
      }
    });
  }
  
  ngOnInit() {   
    this.form = new FormGroup({
      assetName: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    if (this.form.invalid || this.saving()) return;

    const assetName = this.form.get('assetName')?.value as string;
    const file = this.selectedFile();
    const existing = this.asset();

    if (!file) {
      this.uploadError.set('Файл не был выбран');
      return;
    }

    if (existing) {
      this.updated.emit({
        assetId: existing.assetId,
        assetName,
        file
      });
    } else {
      this.created.emit(assetName);
    }
  }

  onCancel() {
    this.cancelled.emit();
  }
}