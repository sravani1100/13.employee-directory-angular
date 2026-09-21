import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { catchError, map, of, tap } from 'rxjs';
import { UserService } from '../shared/services/user';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const userService = inject(UserService);
  const router = inject(Router);

  return authService.getCurrentUser()
    .pipe(

      tap(user => {
        userService.setUser(user);
      }),

      map(() => true),
      catchError(() => {
        router.navigate(['/']);
        return of(false);
      })
    );
};
