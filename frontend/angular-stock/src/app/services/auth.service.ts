import { Injectable, inject } from '@angular/core'
import { CookieService } from 'ngx-cookie-service'
import { Router } from '@angular/router'

// Define the type of user data
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

  // สร้างตัวแปรเก็บข้อมูลผู้ใช้งาน
  userProfile = {
    "username": "",
    "email": "",
    "role": "",
    "token": ""
  }

  // ฟังก์ชันสำหรับเก็บ user ลง cookie
  setUser(user: UserProfile){
    // setUser(user: Record<string, string>){
    const expirationDate = new Date()
    expirationDate.setHours(expirationDate.getHours() + 24)

    this.cookieService.set('LoggedInUser', user['username'], expirationDate)
    this.cookieService.set("LoggedInEmail", user['email'], expirationDate)
    this.cookieService.set("LoggedInRole", user['role'], expirationDate)
    this.cookieService.set("LoggedInToken", user['token'], expirationDate)
  }

  // ฟังก์ชันสำหรับ getUser จาก cookie
  getUser(){
    this.userProfile.username = this.cookieService.get('LoggedInUser') || ""
    this.userProfile.email = this.cookieService.get('LoggedInEmail') || ""
    this.userProfile.role = this.cookieService.get('LoggedInRole') || ""
    this.userProfile.token = this.cookieService.get('LoggedInToken') || ""

    return this.userProfile
  }

  // ฟังก์ชันเช็คสถานะการ Login
  isLoggedIn(){
    return this.getUser().token !== ""
  }

  // ฟังก์ชันสำหรับ Logout
  logout(){
    this.cookieService.delete("LoggedInUser")
    this.cookieService.delete("LoggedInEmail")
    this.cookieService.delete("LoggedInRole")
    this.cookieService.delete("LoggedInToken")
    this.router.navigate(['/login'])
  }

}
