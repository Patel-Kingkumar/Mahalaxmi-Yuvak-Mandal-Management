import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  // Get values from sessionStorage
  const token = sessionStorage.getItem('token');
  const userRole = sessionStorage.getItem('role');

  // Roles allowed for the route
  const allowedRoles: string[] | undefined = route.data?.['roles'];

  // ❌ If token not found → redirect to login
  if (!token) {
    router.navigateByUrl('/login');
    return false;
  }

  // ❌ If route has role restriction and user role not allowed
  if (allowedRoles && userRole && !allowedRoles.includes(userRole)) {

    alert('You do not have permission to access this page');

    router.navigateByUrl('/dashboard');

    return false;
  }

  // ✅ Access allowed
  return true;
};
