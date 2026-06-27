import {  Injectable } from "@angular/core";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";
import { UpdateAssetRequest } from "../../../core/contracts/requests/update-asset.request";
import { ProjectResponse } from "../../../core/contracts/responses/project.response";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";
import { Result } from "@crossdyne/toolkit";
import { HttpService } from "../../../core/http/http.service";

@Injectable({
    providedIn: 'root'
})
export class PersonalAssetApiService extends HttpService{

    constructor(){
        super('api/v1');
    }

    async updateAsync(data: UpdateAssetRequest): Promise<Result<string>> {
        const formData = new FormData();

        formData.append('AssetId', data.assetId);
        formData.append('AssetName', data.assetName);
        formData.append('File', data.file);
        formData.append('CategoryId', data.categoryId);

        return await this.putAsync<string>('/personal/asset', formData);
    }
    
    async createAsync(data: CreateAssetRequest): Promise<Result<string>> {
        const formData = new FormData();

        formData.append('CategoryId', data.categoryId);
        formData.append('ProjectIdsJson', JSON.stringify(data.projectIdsJson));
        formData.append('AssetName', data.assetName);
        formData.append('File', data.file);

        return await this.postAsync<string>('/personal/asset', formData);
    }

    async removeAsync(id: string): Promise<Result<void>> {
        return await this.deleteAsync<void>(`/personal/asset/${id}`)
    }

    async getAllAssetsAsync(): Promise<Result<AssetUrlResponse[]>> {
        return await this.getAsync<AssetUrlResponse[]>('/personal/asset/urls');
    }

    async getProjectsAsync(): Promise<Result<ProjectResponse[]>> {
        return await this.getAsync<ProjectResponse[]>('/project');
    }

    async getCategoriesAsync(): Promise<Result<CategoryResponse[]>> {
        return await this.getAsync<CategoryResponse[]>('/personal/category')
    }
}