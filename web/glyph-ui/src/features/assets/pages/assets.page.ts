import { Component, inject, signal } from "@angular/core";
import { SortButton } from "../../../shared/ui/sort-button/sort-button";
import { AssetListComponent } from "../components/asset-list/asset-list.component";
import { SvgFormComponent } from "../components/svg-form/svg-form.component";
import { FileUploaderComponent } from "../components/file-uploader/file-uploader.component";
import { AssetApiPersonalService } from "../services/asset-api-personal.service";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { UpdateAssetRequest } from "../../../core/contracts/requests/update-asset.request";
import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";

@Component({
    selector: 'asset-page',
    templateUrl: './assets.page.html',
    styleUrls: ['./assets.page.scss'],
    standalone: true,
    imports: [SortButton, AssetListComponent, SvgFormComponent, FileUploaderComponent]
})
export class AssetsPage {
    private http = inject(AssetApiPersonalService);

    selectedFile = signal<File | null>(null);
    uploadError = signal<string | null>(null);
    saving = signal(false);
    assets = signal<AssetUrlResponse[]>([]);
    selectedAsset = signal<AssetUrlResponse | null>(null);
    
    constructor() {
        this.loadAssets();
    }

    loadAssets() {
        this.http.getAllAssets().subscribe({
            next: assets => this.assets.set(assets),
            error: error => console.error(error)
        });
    }

    onFileSelected(files: File[]) {
        const file = files[0];
        if (!file) return;

        if (!file.name.toLowerCase().endsWith('.svg') && file.type !== 'image/svg+xml') {
            this.uploadError.set('Можно загружать только SVG файлы');
            return;
        }

        this.uploadError.set(null);
        this.selectedFile.set(file);
    }

    async onCreate(assetName: string) {
        const file = this.selectedFile();
        if (!file) {
            this.uploadError.set('Файл не был выбран');
            return;
        }

        this.saving.set(true);
        try {
            const request: CreateAssetRequest = {
                file,
                categoryId: 'e0a825c8-0541-4db6-a837-7dc6a0b9597b',
                projectIdsJson: JSON.stringify(['74668ee9-4908-463e-82cd-4cdd92e33870']), 
                assetName
            };

            await this.http.create(request);
            this.resetForm();
            this.loadAssets();
        } catch (error) {
            console.error('Ошибка при создании:', error);
            this.uploadError.set('Ошибка при сохранении на сервере');
        } finally {
            this.saving.set(false);
        }
    }

    async onUpdate(request: UpdateAssetRequest) {
        this.saving.set(true);
        try {
            await this.http.update(request);
            this.resetForm();
            this.loadAssets();
        } catch (error) {
            console.error('Ошибка при обновлении:', error);
            this.uploadError.set('Ошибка при обновлении на сервере');
        } finally {
            this.saving.set(false);
        }
    }

    async onDelete(id: string) {
        this.http.delete(id).subscribe({
            next: () =>{
                this.assets.update(assets => assets.filter(a => a.assetId !== id));

                if (this.selectedAsset()?.assetId === id){
                    this.selectedAsset.set(null);
                }
            },
            error: error => console.error('Ошибка удаления ассета', error)
        })
    }

    onAssetSelected(asset: AssetUrlResponse) {
        this.selectedAsset.set(asset);
        this.selectedFile.set(null);
    }

    onCancel() {
        this.resetForm();
    }

    private resetForm() {
        this.selectedAsset.set(null);
        this.selectedFile.set(null);
        this.uploadError.set(null);
    }

    onUploadError(error: string) {
        this.uploadError.set(error);
    }
}