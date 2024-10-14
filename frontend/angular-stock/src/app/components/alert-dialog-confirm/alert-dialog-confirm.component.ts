import { Component, Inject, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-alert-dialog-confirm',
  standalone: true,
  imports: [MatIcon, MatDialogTitle, MatDialogContent, MatDialogActions, MatButton],
  templateUrl: './alert-dialog-confirm.component.html',
  styleUrls: ['./alert-dialog-confirm.component.scss']
})
export class AlertDialogConfirmComponent {

  @Inject(MAT_DIALOG_DATA)
  public data = inject(MAT_DIALOG_DATA)
  public dialogRef = inject(MatDialogRef<AlertDialogConfirmComponent>)

  // Close the dialog
  closeDialog(): void {
    this.dialogRef.close(false); // Close without confirmation
  }

  // Confirm action
  confirmAction(): void {
    if (this.data.confirmAction) {
      this.data.confirmAction(); // Execute provided action
    }
    this.dialogRef.close(true); // Close with confirmation
  }
}