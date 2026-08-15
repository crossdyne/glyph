import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layout/main/main-layout.component';
import { adminGuard } from '../core/guards/admin.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/assets', pathMatch: 'full'},
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'assets', loadComponent: () => import('../features/assets/pages/personal-assets/personal-assets.page').then(c => c.PersonalAssetsPage) },
            { path: 'categories', loadComponent: () => import('../features/categories/pages/personal-categories/personal-categories.page').then(c => c.CategoriesPage) },
            {
                path: 'admin',
                canActivate: [adminGuard],
                children: [
                    { path: 'assets', loadComponent: () => import('../features/assets/pages/global-assets/global-assets.page').then(c => c.GlobalAssetsPage) },
                    { path: 'categories', loadComponent: () => import('../features/categories/pages/global-categories/global-categories.page').then(c => c.GlobalCategoriesPage) }
                ]
            }
        ]
    }
];
