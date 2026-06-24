import { Component, inject, signal } from "@angular/core";
import { FileUploaderComponent } from "../../components/file-uploader/file-uploader.component";
import { SortButton } from "../../../../shared/ui/sort-button/sort-button";
import { SvgFormComponent } from "../../components/svg-form/svg-form.component";
import { AssetListComponent } from "../../components/asset-list/asset-list.component";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { GlobalAssetApiService } from "../../services/global-asset-api.service";
import { ProjectResponse } from "../../../../core/contracts/responses/project.response";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { CreateAssetRequest } from "../../../../core/contracts/requests/create-asset.request";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";

@Component({
    selector: 'global-assets-page',
    templateUrl: './global-assets.page.html',
    styleUrls: ['./global-assets.page.scss'],
    standalone: true,
    imports: [FileUploaderComponent, SortButton, SvgFormComponent, AssetListComponent]
})
export class GlobalAssetsPage {
     private http = inject(GlobalAssetApiService);

    assets = signal<AssetUrlResponse[]>([]);
    projects = signal<ProjectResponse[]>([]);
    categories = signal<CategoryResponse[]>([]);

    selectedFile = signal<File | null>(null);
    uploadError = signal<string | null>(null);
    saving = signal(false);
    selectedAsset = signal<AssetUrlResponse | null>(null);
    
    constructor() {
        this.loadCategories();
        this.loadProjects();
        this.loadAssets();
    }

    onUploadError(error: string) {
        this.uploadError.set(error);
    }
    
    onAssetSelected(asset: AssetUrlResponse) {
        this.selectedAsset.set(asset);
        this.selectedFile.set(null);
    }

    //#region CRUD - события

    async onCreate(request: CreateAssetRequest) {
        const file = this.selectedFile();
        if (!file) {
            this.uploadError.set('Файл не был выбран');
            return;
        }

        this.saving.set(true);
        try {
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
            next: () => {
                this.assets.update(assets => assets.filter(a => a.assetId !== id));

                if (this.selectedAsset()?.assetId === id) {
                    this.selectedAsset.set(null);
                }
            },
            error: error => console.error('Ошибка удаления ассета', error)
        })
    }

    //#endregion

    //#region Получение данных

    loadAssets() {
        this.http.getAllAssets().subscribe({
            next: assets => this.assets.set(assets),
            error: error => console.error(error)
        });
    }

    loadProjects() {
        this.http.getProjects().subscribe({
            next: projects => this.projects.set(projects),
            error: error => console.error(error) 
        })
    }

    loadCategories() {
        this.http.getCategories().subscribe({
            next: categories => this.categories.set(categories),
            error: error => console.error(error)
        });

        // const logData = categories.map(p => ({ id: p.categoryId, name: p.name}));
        // console.table(logData);
    }

    //#endregion

    //#region События очистки, отмены

    onCancel() {
        this.resetForm();
    }

    private resetForm() {
        this.selectedAsset.set(null);
        this.selectedFile.set(null);
        this.uploadError.set(null);
    }

    //#endregion
}