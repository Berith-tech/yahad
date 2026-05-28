import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reset-password-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './reset-password-form.component.html',
})
export class ResetPasswordFormComponent {
  @Input() loading = false;
  @Input() errorMessage = '';
  @Input() successMessage = '';
  @Output() submitted = new EventEmitter<string>();

  email = '';

  onSubmit(): void {
    this.submitted.emit(this.email);
  }
}
