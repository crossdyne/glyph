import { Component, inject, OnInit, signal } from "@angular/core";
import { CategoryListComponent } from "../../components/category-list/category-list.component";
import { CategoryFormComponent } from "../../components/category-form/category-form.component";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { PersonalCategoriesApiService } from "../../services/personal-category-api.service";
import { CreateCategoryRequest } from "../../../../core/contracts/requests/create-category.request";
import { UpdateCategoryRequest } from "../../../../core/contracts/requests/update-category.request";
import { from } from "linq-to-typescript";

@Component({
    selector: 'personal-categories-page',
    templateUrl: './personal-categories.page.html',
    styleUrls: ['./personal-categories.page.scss'],
    standalone: true,
    imports: [CategoryListComponent, CategoryFormComponent]
})
export class CategoriesPage implements OnInit{
    private categoryService = inject(PersonalCategoriesApiService)

    categories = signal<CategoryResponse[]>([]);

    selectedCategory = signal<CategoryResponse | null>(null);

    loading = signal(true);
    error = signal('');

    ngOnInit(): void {
        this.loadCategories();
    }

    //#region CRUD

    loadCategories(): void {
        this.loading.set(true);
        this.error.set('');

        this.categoryService.getAll().subscribe({
            next: data => {
                const result = from(data).orderBy(x => x.name).toArray();
                this.categories.set(result);
                this.loading.set(false);
            },
            error: err => {
                this.error.set('Ошибка загрузка категорий');
                this.loading.set(false);
                console.error(err);
            }
        });
    }
    
    onCreate(request: CreateCategoryRequest): void {
        this.categoryService.create(request).subscribe({
            next: (newId) => {
                const newCat: CategoryResponse = {
                    categoryId: newId,
                    name: request.name,
                    isPublic: false
                }

                const update = [newCat, ...this.categories()];
                this.categories.set(this.sortCategories(update));
                this.selectedCategory.set(null);
            }
        })
    }

    onUpdate(request: UpdateCategoryRequest): void {
        this.categoryService.update(this.selectedCategory()?.categoryId!, request).subscribe({
            next: () => {
                const updateCat: CategoryResponse = {
                    categoryId: this.selectedCategory()?.categoryId!,
                    name: request.name,
                    isPublic: false
                }
                
                const update = this.categories().map(c => c.categoryId === this.selectedCategory()?.categoryId!? updateCat : c);
                
                this.categories.set(this.sortCategories(update));
                this.selectedCategory.set(null);
            }
        })
    }

    onDelete(id: string): void {
        this.categoryService.delete(id).subscribe({
            next: () => {
                this.categories.update(cats => cats.filter(c => c.categoryId !== id));

                if (this.selectedCategory()?.categoryId === id){
                    this.selectedCategory.set(null);
                }
            },
            error: (err) => {
                console.error('Ошибка удаления категории', err);
            }
        });
    }

    onEdit(id: string): void {
        const category = this.categories().find(c => c.categoryId === id);
        if (category) {
            this.selectedCategory.set(category);
        }
    }

    //#endregion

    //#region Сортировка

    sortCategories(data: CategoryResponse[]): CategoryResponse[] {
        return from(data).orderBy(x => x.name).toArray();
    }

    //#endregion

    onCancel(): void {
        this.selectedCategory.set(null);
    }
}