import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserService } from '../shared/services/user';

export const adminGuard: CanActivateFn = () => {
  const userService = inject(UserService);
  const router = inject(Router);

  const user = userService.user();
  if(user?.role === 'Admin'){
    return true;
  }

  router.navigate(['/dashboard']);

  return false;
};
