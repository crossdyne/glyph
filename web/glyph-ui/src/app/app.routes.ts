import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layout/main/main-layout.component';

export const routes: Routes = [
    { path: '', redirectTo: '/assets', pathMatch: 'full'},
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'assets', loadComponent: () => import('../features/assets/pages/assets.page').then(c => c.AssetsPage) },
            { path: 'categories', loadComponent: () => import('../features/categories/pages/categories.page').then(c => c.CategoriesPage) },
        ]
    }
];
