import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CreateCategoryRequest } from "../../../core/contracts/requests/create-category.request";
import { Observable } from "rxjs";
import { UpdateCategoryRequest } from "../../../core/contracts/requests/update-category.request";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";

@Injectable({
    providedIn: 'root'
})
export class GlobalCategoriesApiService {
    private http = inject(HttpClient);

    private readonly pathUrl: string = '/api/v1/global/category';

    create(data: CreateCategoryRequest): Observable<string> {
        return this.http.post<string>(`${this.pathUrl}`, data);
    }

    update(id: string, data: UpdateCategoryRequest): Observable<void> {
        return this.http.patch<void>(`${this.pathUrl}/${id}`, data);
    }

    delete(id: string): Observable<void>{
        return this.http.delete<void>(`${this.pathUrl}/${id}`);
    }

    getAll(): Observable<CategoryResponse[]>{
        return this.http.get<CategoryResponse[]>(`${this.pathUrl}`);
    }
}