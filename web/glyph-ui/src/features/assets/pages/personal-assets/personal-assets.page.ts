import { Component, computed, inject, signal } from "@angular/core";
import { AssetListComponent } from "../../components/asset-list/asset-list.component";
import { SvgFormComponent } from "../../components/svg-form/svg-form.component";
import { FileUploaderComponent } from "../../components/file-uploader/file-uploader.component";
import { CreateAssetRequest } from "../../../../core/contracts/requests/create-asset.request";
import { UpdateAssetRequest } from "../../../../core/contracts/requests/update-asset.request";
import { AssetUrlResponse } from "../../../../core/contracts/responses/asset-urls.response";
import { ProjectResponse } from "../../../../core/contracts/responses/project.response";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { PersonalAssetApiService } from "../../services/personal-asset-api.service";
import { ErrorList, Result } from "@crossdyne/toolkit";
import { Grouping } from "../../utils/grouping.utils";
import { Sorting } from "../../utils/sorting.utils";
import { Dialog } from "@angular/cdk/dialog";
import { ConfirmFormResult } from "../../../../shared/ui/confirm-form/model/confirm-form.result";
import { ConfirmFormData } from "../../../../shared/ui/confirm-form/model/confirm-form.data";
import { ConfirmFormComponent } from "../../../../shared/ui/confirm-form/confirm-form";

@Component({
    selector: 'personal-assets-page',
    templateUrl: './personal-assets.page.html',
    styleUrls: ['./personal-assets.page.scss'],
    standalone: true,
    imports: [AssetListComponent, SvgFormComponent, FileUploaderComponent]
})
export class PersonalAssetsPage {
    private dialog = inject(Dialog);
    private http = inject(PersonalAssetApiService);

    assets = signal<AssetUrlResponse[]>([]);
    groupedAssets = computed(() => Grouping.groupedAssets(this.categories(), this.assets()));
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

        const result: Result<string> = await this.http.createAsync(request);

        result.match(
            id => {
                this.resetForm();
                this.loadAssets();
            },
            errors => {
                console.error('Ошибка создания ассета:', this.mapErrors(errors));
                this.uploadError.set('Ошибка при сохранении на сервере');
            }
        );

       this.saving.set(false);
    }

    async onUpdate(request: UpdateAssetRequest) {
        this.saving.set(true);

        const result: Result<string> = await this.http.updateAsync(request); 

        result.match(
            id => {
                this.resetForm();
                this.loadAssets();
            },
            errors =>{
                console.error('Ошибка обновления ассета:', this.mapErrors(errors));
                this.uploadError.set('Ошибка при сохранении на сервере');
            } 
        );
            
        this.saving.set(false);
    }

    async onDelete(id: string) {
        const dialogRef = this.dialog.open<ConfirmFormResult, ConfirmFormData, ConfirmFormComponent>(
            ConfirmFormComponent, {
                disableClose: true,
                hasBackdrop: true,
                data: {
                    title: `Вы точно хотите удалить асет: "${this.assets().find(c => c.assetId === id)?.assetName}"?`,
                    body: 'После этого действия данный асет пропадет со всех доступных сайтов!'
                }
            }
        );

        dialogRef.closed.subscribe(async result => {
            if (!result)
                return;

            if (result.status === 'ok'){
                const result: Result<void> = await this.http.removeAsync(id);
                
                result.match(
                    () => {
                        this.assets.update(assets => assets.filter(a => a.assetId !== id));

                        if (this.selectedAsset()?.assetId === id) {
                            this.selectedAsset.set(null);
                        }
                    },
                    errors => console.error('Ошибка удаления асета:', this.mapErrors(errors))
                );
            }
        });
    }

    //#endregion

    //#region Получение данных

    async loadAssets() {
        const result: Result<AssetUrlResponse[]> = await this.http.getAllAssetsAsync();

        result.match(
            assets => this.assets.set(assets),
            errors => console.error('Ошибка загрузки ассетов:', this.mapErrors(errors))
        );
    }

    async loadProjects() {
        const result: Result<ProjectResponse[]> = await this.http.getProjectsAsync();

        result.match(
            projects => this.projects.set(projects),
            errors => console.error('Ошибка загрузки проектов:', this.mapErrors(errors))
        );
    }

    async loadCategories() {
        const result: Result<CategoryResponse[]> = await this.http.getCategoriesAsync();

        result.match(
            categories => this.categories.set(Sorting.sortCategoriesAlphabet(categories)),
            errors => console.error('Ошибка загрузки категорий:', this.mapErrors(errors))
        );
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

    //#region Хелперы

    private mapErrors(errors: ErrorList): string{
        return errors.map(e => e.message).join(', ')
    }

    //#endregion

}