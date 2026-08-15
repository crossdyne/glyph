import { Injectable } from "@angular/core";
import { UpdateAssetRequest } from "../../../core/contracts/requests/update-asset.request";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";
import { ProjectResponse } from "../../../core/contracts/responses/project.response";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";

@Injectable({
    providedIn: 'root'
})
export class GlobalAssetApiService extends HttpService {

    constructor(){
        super('api/v1')
    }

    async updateAsync(data: UpdateAssetRequest): Promise<Result<string>> {
        const formData = new FormData();

        formData.append('AssetId', data.assetId);
        formData.append('AssetName', data.assetName);
        formData.append('CategoryId', data.categoryId);

        if (data.file)
            formData.append('File', data.file);

        return await this.putAsync<string>('/global/asset', formData);
    }
    
    async createAsync(data: CreateAssetRequest): Promise<Result<string>> {
        const formData = new FormData();

        formData.append('CategoryId', data.categoryId);
        formData.append('ProjectIdsJson', JSON.stringify(data.projectIdsJson));
        formData.append('AssetName', data.assetName);
        formData.append('File', data.file);

        return await this.postAsync<string>('/global/asset', formData);
    }

    async removeAsync(id: string): Promise<Result> {
        return await this.deleteAsync<void>(`/global/asset/${id}`);
    }

    async getAllAssetsAsync(): Promise<Result<AssetUrlResponse[]>> {
        return await this.getAsync<AssetUrlResponse[]>('/global/asset/urls');
    }

    async getAllProjectsAsync(): Promise<Result<ProjectResponse[]>> {
        return await this.getAsync<ProjectResponse[]>('/project')
    }

    async getCategoriesAsync(): Promise<Result<CategoryResponse[]>> {
        return await this.getAsync<CategoryResponse[]>('/global/category')
    }
}