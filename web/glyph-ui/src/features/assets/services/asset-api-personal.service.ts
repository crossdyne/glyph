import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { firstValueFrom, Observable } from "rxjs";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";
import { UpdateAssetRequest } from "../../../core/contracts/requests/update-asset.request";

@Injectable({
    providedIn: 'root'
})
export class AssetApiPersonalService {
    private http = inject(HttpClient);

    private readonly pathUrl: string = '/api/v1/personal/asset';

    async update(data: UpdateAssetRequest): Promise<string> {
        const formData = new FormData();

        formData.append('AssetId', data.assetId);
        formData.append('AssetName', data.assetName);
        formData.append('File', data.file);

        return await firstValueFrom(this.http.put<string>(`${this.pathUrl}`, formData));
    }
    
    async create(data: CreateAssetRequest): Promise<string> {
        const formData = new FormData();

        formData.append('CategoryId', data.categoryId);
        formData.append('ProjectIdsJson', data.projectIdsJson);
        formData.append('AssetName', data.assetName);
        formData.append('File', data.file);

        return await firstValueFrom(this.http.post<string>(`${this.pathUrl}`, formData));
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.pathUrl}/${id}`);
    }

    getAllAssets(): Observable<AssetUrlResponse[]> {
        return this.http.get<AssetUrlResponse[]>(`${this.pathUrl}/urls`);
    }
}