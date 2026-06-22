import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { firstValueFrom, Observable } from "rxjs";
import { CreateAssetRequest } from "../../../core/contracts/requests/create-asset.request";
import { AssetUrlResponse } from "../../../core/contracts/responses/asset-urls.response";

@Injectable({
    providedIn: 'root'
})
export class AssetApiPersonalService {
    private http = inject(HttpClient);

    private readonly pathUrl: string = '/api/v1/personal/asset';

    update(assetId: string, svgCode: string) {
        throw new Error("Method not implemented.");
    }
    
    async create(data: CreateAssetRequest): Promise<string> {
        const formData = new FormData();

        formData.append('CategoryId', data.categoryId);
        formData.append('ProjectIdsJson', data.projectIdsJson);
        formData.append('File', data.file);

        return await firstValueFrom(this.http.post<string>(`${this.pathUrl}`, formData));
    }

    getAllAssets(): Observable<AssetUrlResponse[]> {
        return this.http.get<AssetUrlResponse[]>(`${this.pathUrl}/urls`);
    }
}