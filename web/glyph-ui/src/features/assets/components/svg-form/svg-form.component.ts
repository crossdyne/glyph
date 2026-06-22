import { Component, effect, inject, input, OnInit, output, signal } from "@angular/core";
import { AssetResponse } from "../../../../core/contracts/responses/asset.response";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";

@Component({
    selector: 'svg-form',
    templateUrl: './svg-form.component.html',
    styleUrls: ['./svg-form.component.scss'],
    standalone: true,
    imports: [ReactiveFormsModule],
})
export class SvgFormComponent implements OnInit {
  
  asset = input<AssetResponse | null>(null);
  externalSvgCode = input<string | null>(null);

  created = output<string>(); 
  updated = output<UpdateAssetRequest>();
  cancelled = output<void>();

  form!: FormGroup;
  svgPreviewUrl: SafeResourceUrl = '';
  name = signal('');
  saving = signal(false);
  
  private currentBlobUrl: string | null = null;

  constructor(private sanitizer: DomSanitizer) {
    effect(() => {
      const code = this.externalSvgCode();
      if (code !== null && code !== undefined) {
        this.form?.get('svgCode')?.setValue(code, { emitEvent: false });
        this.updatePreview();
      }
    });
  }
  
  ngOnInit() {   
    this.form = new FormGroup({
      svgCode: new FormControl(this.asset()?.svgCode || '', Validators.required),
      assetName: new FormControl(this.name() || '', Validators.required)
    });
    
    this.updatePreview();
    
    this.form.get('svgCode')?.valueChanges.subscribe(() => this.updatePreview());
  }
    
  updatePreview() {
    if (this.currentBlobUrl){
      URL.revokeObjectURL(this.currentBlobUrl);
      this.currentBlobUrl = null;
    }

    const svgCode = this.form.get('svgCode')?.value as string;

    if (!svgCode) {
      this.svgPreviewUrl = '';
      return;
    }

    const blob = new Blob([svgCode], { type: 'image/svg+xml' });
    this.currentBlobUrl = URL.createObjectURL(blob);
    this.svgPreviewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.currentBlobUrl);
  }

  onSubmit() {
    if (this.form.invalid || this.saving())
        return;

    const svgCode = this.form.get('svgCode')?.value as string;
    const assetName = this.form.get('assetName')?.value as string;
    const existing = this.asset();

    if (existing) {
      this.updated.emit({ assetId: existing.assetId, svgCode, assetName } as UpdateAssetRequest);
      this.svgPreviewUrl = '';
    } else {
      this.created.emit(assetName);
      this.svgPreviewUrl = '';
    }
  }

  onCancel() {
    this.cancelled.emit();
    this.svgPreviewUrl = '';
  }
}