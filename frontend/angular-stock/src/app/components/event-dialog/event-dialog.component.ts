import { Component, Inject, inject } from '@angular/core'
import { CommonModule } from '@angular/common'
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
  selector: 'app-event-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatIcon,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButton,
  ],
  templateUrl: './event-dialog.component.html',
  styleUrl: './event-dialog.component.scss'
})
export class EventDialogComponent {

  @Inject(MAT_DIALOG_DATA)
  public data = inject(MAT_DIALOG_DATA)
  public dialogRef = inject(MatDialogRef<EventDialogComponent>)

  // Close the dialog
  closeDialog(): void {
    this.dialogRef.close(null)
  }

}
