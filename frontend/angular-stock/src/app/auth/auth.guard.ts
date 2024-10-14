import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const auth = inject(AuthService);

  if (auth.isLoggedIn()) {
    if (state.url == '/login' || state.url == '/register') {
      router.navigate(['dashboard']);
    }
    return true;
  } else {
    if (state.url != '/login' && state.url != '/register') {
      router.navigate(['login']);
    }
    return true;
  }
};

export const AdminauthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const auth = inject(AuthService);

  if (auth.isAdmin()) {
    switch (state.url) {
      case '/login':
      case '/register':
        router.navigate(['dashboard']);
        break;
      case '/Stock':
        router.navigate(['admin-dashboard']);
        break;
      case '/Event':
        router.navigate(['admin-settings']);
        break;
      default:
        router.navigate(['dashboard']);
    }
    return true;
  } else {
    if (state.url != '/login' && state.url != '/register') {
      router.navigate(['login']);
    }
    return true;
  }
};
