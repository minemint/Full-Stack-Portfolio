import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { AuthService } from './../services/auth.service'

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router)
  const auth = inject(AuthService)

  // ถ้า login แล้วให้ไปหน้าที่ต้องการ
  if(auth.isLoggedIn()){
    if (state.url == '/login' || state.url == '/register') {
      router.navigate(['dashboard'])
    }
    return true
  }
  // ถ้ายังไม่ login ให้ไปหน้า login
  else{
    if (state.url != '/login' && state.url != '/register') {
      router.navigate(['login'])
    }
    return true
  }

}
