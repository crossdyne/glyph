import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";

@Injectable({
    providedIn: 'root'
})
export class DisplayCategoryApi{
    private http = inject(HttpClient);

    getAll(): Observable<CategoryResponse[]>{
        return this.http.get<CategoryResponse[]>('/api/v1/personal/categories');
    }
}