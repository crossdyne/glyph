import { Component, inject, signal } from "@angular/core";
import { SortButton } from "../../../shared/ui/sort-button/sort-button";
import { AssetListComponent } from "../components/asset-list/asset-list.component";
import { SvgFormComponent } from "../components/svg-form/svg-form.component";
import { FileUploaderComponent } from "../components/file-uploader/file-uploader.component";
import { AssetApiPersonalService } from "../services/asset-api-personal.service";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { UpdateAssetRequest } from "../../../core/contracts/requests/update-asset.request";
import { toSignal } from "@angular/core/rxjs-interop";
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

    selectedFile = signal<File |null>(null);
    selectedSvgCode = signal<string | null>(null);
    uploadError = signal<string | null>(null);
    saving = signal(false);
    assets = signal<AssetUrlResponse[]>([]);
    
    // readonly assets = toSignal(this.http.getAllAssets(), { initialValue: [] });

    constructor(){
        this.http.getAllAssets().subscribe({
            next: assets => this.assets.set(assets),
            error: error => console.error(error)
        });
    }

    onFileSelected(files: File[]) {
        const file = files[0];
        if (!file) 
            return;

        if (!file.name.toLowerCase().endsWith('.svg') && file.type !== 'image/svg+xml') {
            this.uploadError.set('Можно загружать только SVG файлы');
            return;
        }

        this.uploadError.set(null);
        this.selectedFile.set(file);

        const reader = new FileReader();
        reader.onload = () => this.selectedSvgCode.set(reader.result as string);
        reader.onerror = () => this.uploadError.set('Ошибка чтения файла');
        reader.readAsText(file);
    }

    async onCreate() {
        const file = this.selectedFile();
        if (!file){
            this.uploadError.set('Файл не был выбран');
            return;
        }

        this.saving.set(true);
        try {
            const request: CreateAssetRequest = {
                file: file,
                categoryId: 'e0a825c8-0541-4db6-a837-7dc6a0b9597b',
                projectIdsJson: JSON.stringify(['74668ee9-4908-463e-82cd-4cdd92e33870']), 
            }

            await this.http.create(request);

            this.selectedSvgCode.set(null);
            this.selectedFile.set(null); 
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
            await this.http.update(request.assetId, request.svgCode);
        } catch (error) {
            this.saving.set(false);
        }
    }

    onUploadError(error: string) {
        this.uploadError.set(error);
    }
}