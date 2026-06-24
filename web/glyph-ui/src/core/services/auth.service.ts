import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, map, Observable, of, shareReplay } from "rxjs";
import { UserAuthResponse } from "../contracts/responses/user-auth.response";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private userProfile$?: Observable<UserAuthResponse | null>;
    
    getUserProfile(): Observable<UserAuthResponse | null> {
      if (!this.userProfile$) {
        this.userProfile$ = this.http.get<UserAuthResponse>('/api/v1/me').pipe(
          catchError(() => of(null)),
          shareReplay(1)
        );
      }
      return this.userProfile$;
    }
  
    hasRole(role: string): Observable<boolean> {
      return this.getUserProfile().pipe(
        map(user => user?.roles?.includes(role) ?? false)
      );
    }
    
    hasAnyRole(roles: string[]): Observable<boolean> {
     return this.getUserProfile().pipe(
       map(user => {
         if (!user?.roles) 
            return false;
         
         const userRolesLower = user.roles.map(r => r.toLowerCase());
         const requiredRolesLower = roles.map(r => r.toLowerCase());

         return requiredRolesLower.some(role => userRolesLower.includes(role));
       })
     );
   }
  
    isAdmin(): Observable<boolean> {
      return this.hasAnyRole(['Admin', 'SuperAdmin']); 
    }
    
    logout(): void {
        this.userProfile$ = undefined;
        window.location.href = environment.returnAuthUrlBase;
  }
}