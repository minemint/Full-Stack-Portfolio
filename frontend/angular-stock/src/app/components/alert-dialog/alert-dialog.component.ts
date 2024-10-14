import { Component, Inject, inject } from '@angular/core'
import { MatButton } from '@angular/material/button'
import { MatIcon } from '@angular/material/icon'
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
} from '@angular/material/dialog'

@Component({
  selector: 'app-alert-dialog',
  standalone: true,
  imports: [
    MatIcon,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButton,
  ],
  templateUrl: './alert-dialog.component.html',
  styleUrl: './alert-dialog.component.scss'
})
export class AlertDialogComponent {

  @Inject(MAT_DIALOG_DATA)
  public data = inject(MAT_DIALOG_DATA)
  public dialogRef = inject(MatDialogRef<AlertDialogComponent>)

  // Close the dialog
  closeDialog(): void {
    this.dialogRef.close(null)
  }

}
