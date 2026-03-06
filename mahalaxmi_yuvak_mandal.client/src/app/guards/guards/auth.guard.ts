import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  const token = sessionStorage.getItem('token');
  const userRole = sessionStorage.getItem('role');
  const allowedRoles = route.data?.['roles'];

  // If token not found → redirect to login
  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  // If route has role restriction
  if (allowedRoles && !allowedRoles.includes(userRole)) {
    alert('You do not have permission to access this page');
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};