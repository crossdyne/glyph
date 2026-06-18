import { Component, effect, inject, input, OnInit, output } from "@angular/core";
import { CreateCategoryRequest } from "../../../../core/contracts/requests/create-category.request";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { UpdateCategoryRequest } from "../../../../core/contracts/requests/update-category.request";

@Component({
    selector: 'category-form',
    templateUrl: './category-form.component.html',
    styleUrls: ['./category-form.component.scss'],
    standalone: true,
    imports: [ReactiveFormsModule]
})
export class CategoryFormComponent {
    private fb = inject(FormBuilder);

    category = input<CategoryResponse | null>(null);

    created = output<CreateCategoryRequest>();
    updated = output<UpdateCategoryRequest>();
    cancelled = output<void>();

    form = this.fb.group({
        name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]]
    })

    constructor() {
        effect(() => {
            const cat = this.category();

            if (cat) {
                this.form.patchValue({ name: cat.name });
            } else {
                this.form.reset();
            }
        })
    }

    get nameControl() {
        return this.form.controls.name;
    }

    onSubmit(): void {
        if (this.form.invalid){
            this.form.markAllAsTouched();
            return;
        }

        const name = this.form.value.name!.trim();
        const currentCategory = this.category();

        if (currentCategory) {
            const request: UpdateCategoryRequest = {
                name: name
            };
            this.updated.emit(request);
        } else {
            const request: CreateCategoryRequest = {
                name: name
            };
            this.created.emit(request);
        }
    }

    onCancel(): void {
        this.form.reset();
        this.cancelled.emit();
    }
}