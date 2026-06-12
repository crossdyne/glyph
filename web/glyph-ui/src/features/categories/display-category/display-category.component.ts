import { Component, inject, OnInit, signal } from "@angular/core";
import { DisplayCategoryApi } from "./display-category.api";
import { CategoryResponse } from "../../../core/contracts/responses/category.response";

@Component({
    selector: 'display-category',
    templateUrl: 'display-category.component.html',
    styleUrls: ['display-category.component.scss'],
    standalone: true
})
export class DisplayCategoryComponent implements OnInit {
    private http = inject(DisplayCategoryApi);

    categories = signal<CategoryResponse[]>([]);
    loading = signal(true);
    error = signal('');

    ngOnInit(): void {
        const categories = this.http.getAll().subscribe({
            next: data => {
                for (let index = 0; index < data.length; index++) {
                    const element = data[index];
                    console.log(element);
                }
                this.categories.set(data);
                this.loading.set(false);
            },
            error: err =>{
                this.error.set('Ошибка загрузки');
                this.loading.set(false);
            }
        })
    }
}