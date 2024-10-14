import { Injectable, inject } from '@angular/core'
import { CookieService } from 'ngx-cookie-service'
import { Router } from '@angular/router'

type UserProfile = {
  username: string
  email: string
  role: string
  token: string
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private router = inject(Router)
  private cookieService = inject(CookieService)

  userProfile = {
    "username": "",
    "email": "",
    "role": "",
    "token": "" 
  }

  setUser(user: UserProfile){
    const expirationDate = new Date()
    expirationDate.setHours(expirationDate.getHours() + 24)

    this.cookieService.set('LoggedInUser', user['username'], expirationDate)
    this.cookieService.set("LoggedInEmail", user['email'], expirationDate)
    this.cookieService.set("LoggedInRole", user['role'], expirationDate)
    this.cookieService.set("LoggedInToken", user['token'], expirationDate)
  }

  getUser(){
    this.userProfile.username = this.cookieService.get('LoggedInUser') || ""
    this.userProfile.email = this.cookieService.get('LoggedInEmail') || ""
    this.userProfile.role = this.cookieService.get('LoggedInRole') || ""
    this.userProfile.token = this.cookieService.get('LoggedInToken') || ""

    return this.userProfile
  }

  isLoggedIn(){
    return this.getUser().token !== ""
  }
  isAdmin(){
    return this.getUser().role == "admin" || this.getUser().role == "manager"
  }
  logout(){
    this.cookieService.delete("LoggedInUser")
    this.cookieService.delete("LoggedInEmail")
    this.cookieService.delete("LoggedInRole")
    this.cookieService.delete("LoggedInToken")
    this.router.navigate(['/login'])
  }
  
}
