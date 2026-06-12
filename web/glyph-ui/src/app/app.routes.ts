import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: 'categories', loadComponent: () => import('../features/categories/display-category/display-category.component').then(c => c.DisplayCategoryComponent)}
];
