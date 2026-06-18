import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: 'categories', loadComponent: () => import('../features/categories/pages/categories.page').then(c => c.CategoriesPage)}
];
