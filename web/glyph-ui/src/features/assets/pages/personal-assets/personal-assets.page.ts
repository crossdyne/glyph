import { Component, inject, signal } from "@angular/core";
import { SortButton } from "../../../../shared/ui/sort-button/sort-button";
import { AssetListComponent } from "../../components/asset-list/asset-list.component";
import { SvgFormComponent } from "../../components/svg-form/svg-form.component";
import { FileUploaderComponent } from "../../components/file-uploader/file-uploader.component";
import { CreateAssetRequest } from "../../../../core/contracts/requests/create-asset.request";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { ProjectResponse } from "../../../../core/contracts/responses/project.response";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { PersonalAssetApiService } from "../../services/personal-asset-api.service";

@Component({
    selector: 'personal-assets-page',
    templateUrl: './personal-assets.page.html',
    styleUrls: ['./personal-assets.page.scss'],
    standalone: true,
    imports: [SortButton, AssetListComponent, SvgFormComponent, FileUploaderComponent]
})
export class PersonalAssetsPage {
    private http = inject(PersonalAssetApiService);

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