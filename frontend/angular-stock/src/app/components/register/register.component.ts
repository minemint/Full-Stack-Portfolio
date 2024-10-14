import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatIconButton, MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import {
  MatFormField,
  MatLabel,
  MatSuffix,
} from '@angular/material/form-field';
import {
  MatCard,
  MatCardImage,
  MatCardHeader,
  MatCardTitle,
  MatCardContent,
  MatCardActions,
} from '@angular/material/card';
import { Meta } from '@angular/platform-browser';
import { UserService } from '../../services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component'; // Import AlertDialogComponent
import { AuthService } from '../../services/auth.service'; // Import AuthService

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true,
  imports: [
    MatCard,
    MatCardImage,
    MatCardHeader,
    MatCardTitle,
    FormsModule,
    ReactiveFormsModule,
    MatCardContent,
    MatFormField,
    MatLabel,
    MatInput,
    MatIcon,
    MatSuffix,
    MatIconButton,
    MatCardActions,
    MatButton,
  ],
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  submitted: boolean = false;

  // Variables สำหรับรับค่าจากฟอร์ม
  userData = {
    username: '',
    email: '',
    password: '',
  };

  // สำหรับซ่อนแสดง password
  hide = true;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private meta: Meta,
    private https: UserService,
    private dialog: MatDialog, // Inject MatDialog
    private auth: AuthService // Inject AuthService
  ) {}

  ngOnInit(): void {
    // กำหนด Meta Tag description
    this.meta.addTag({
      name: 'description',
      content: 'Login page for Stock Management',
    });

    // Validate form
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  // ฟังก์ชัน Submit สำหรับ Register
  submitRegister() {
    this.submitted = true;

    // ตรวจสอบความถูกต้องของฟอร์ม
    if (this.registerForm.invalid) {
      return;
    } else {
      // เก็บค่าจากฟอร์มลง userData
      this.userData.username = this.registerForm.value.username;
      this.userData.email = this.registerForm.value.email;
      this.userData.password = this.registerForm.value.password;

      // เรียก API สำหรับการลงทะเบียน
      this.https.Register(this.userData).subscribe({
        next: (data: any) => {
          // ตรวจสอบว่าการลงทะเบียนสำเร็จ (ควรตรวจสอบตามโครงสร้างข้อมูลที่ API ส่งกลับ)
          console.log('data: ', data, 'userData2: ', this.userData);
          if (data.email == null && data.email == undefined) {
            // show dialog สมัครสมาชิกสำเร็จ
            this.dialog.open(AlertDialogComponent, {
              data: {
                title: 'สมัครสมาชิกสำเร็จ',
                icon: 'check_circle',
                iconColor: 'green',
                subtitle: 'กำลังเปลี่ยนหน้าไปหน้าหลัก...',
              },
              disableClose: true,
            });

            // เก็บค่าลงตัวแปร userRegister
            const userRegister = {
              username: data.userData.username,
              email: data.userData.email,
              role: data.userData.roles ? data.userData.roles[0] : '',
              token: data.token,
            };

            // เก็บข้อมูลผู้ใช้งานลง cookie
            this.auth.setUser(userRegister);

            // ส่งไปหน้า Home (หลังจาก delay 2 วินาที)
            setTimeout(() => {
              window.location.href = '/dashboard';
            }, 2000);
          } else {
            // หากข้อมูลไม่สมบูรณ์
            this.dialog.open(AlertDialogComponent, {
              data: {
                title: 'มีข้อผิดพลาด',
                icon: 'error',
                iconColor: 'red',
                subtitle: 'ข้อมูลสมัครสมาชิกไม่ถูกต้อง',
              },
              disableClose: true,
            });
          }
        },
        error: (error) => {
          // แสดงข้อความเมื่อเกิดข้อผิดพลาดจาก API
          console.log('error', error);
          this.dialog.open(AlertDialogComponent, {
            data: {
              title: 'มีข้อผิดพลาด',
              icon: 'error',
              iconColor: 'red',
              subtitle: 'ไม่สามารถสมัครสมาชิกได้ กรุณาลองใหม่',
            },
            disableClose: true,
          });
        },
      });

      console.log('userData: ', this.userData);
    }
  }

  onClickCancel() {
    this.router.navigate(['/login']);
  }
}
