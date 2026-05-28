import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-confirm-password-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './confirm-password-form.component.html',
})
export class ConfirmPasswordFormComponent {
  @Input() loading = false;
  @Input() errorMessage = '';
  @Input() successMessage = '';
  @Output() submitted = new EventEmitter<string>();

  newPassword = '';
  confirmPassword = '';
  validationError = '';

  onSubmit(): void {
    this.validationError = '';
    if (this.newPassword !== this.confirmPassword) {
      this.validationError = 'As senhas não coincidem.';
      return;
    }
    this.submitted.emit(this.newPassword);
  }
}
