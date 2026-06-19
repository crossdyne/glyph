import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layout/main/main-layout.component';

export const routes: Routes = [
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'categories', loadComponent: () => import('../features/categories/pages/categories.page').then(c => c.CategoriesPage)}
        ]
    }
];
