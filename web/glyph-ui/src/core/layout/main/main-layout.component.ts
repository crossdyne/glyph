import { Component, inject, ViewEncapsulation } from "@angular/core";
import { RouterOutlet, RouterLinkWithHref, RouterLinkActive } from "@angular/router";
import { environment } from "../../../environments/environment";
import { AuthService } from "../../services/auth.service";
import { toSignal } from "@angular/core/rxjs-interop";

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    styleUrl: './main-layout.component.scss',
    standalone: true,
    encapsulation: ViewEncapsulation.None,
    imports: [RouterOutlet, RouterLinkWithHref, RouterLinkActive]
})
export class MainLayoutComponent{
    private authService = inject(AuthService);

    isAdmin = toSignal(this.authService.isAdmin(), { initialValue: false });

    async logout() {
        const currentUrl = encodeURIComponent(window.location.href);
        window.location.href = `${environment.returnAuthUrlBase}?logout=true&returnUrl=${currentUrl}`;
    } 
}