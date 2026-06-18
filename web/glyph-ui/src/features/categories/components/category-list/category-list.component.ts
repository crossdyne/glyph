import { Component, input, output } from "@angular/core";
import { CategoryResponse } from "../../../../core/contracts/responses/category.response";
import { DeleteButton } from "../../../../shared/ui/delete-button/delete-button";
import { EditButton } from "../../../../shared/ui/edit-button/edit-button";

@Component({
    selector: 'display-category',
    templateUrl: 'category-list.component.html',
    styleUrls: ['category-list.component.scss'],
    standalone: true,
    imports: [DeleteButton, EditButton]
})
export class CategoryListComponent{
    categories = input.required<CategoryResponse[]>();

    delete = output<string>();
    edit = output<string>();

    onDelete(id: string): void {
        this.delete.emit(id);
    }

    onEdit(id: string): void {
        this.edit.emit(id);
    }
}