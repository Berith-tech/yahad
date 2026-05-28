import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ResetPasswordFormComponent } from '../reset-password-dumb/reset-password-form.component';
import { ConfirmPasswordFormComponent } from '../confirm-password-dumb/confirm-password-form.component';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ResetPasswordFormComponent, ConfirmPasswordFormComponent],
  templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent implements OnInit {
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  token = signal('');

  ngOnInit(): void {
    this.token.set(this.route.snapshot.queryParamMap.get('token') ?? '');
  }

  onForgotPassword(email: string): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.forgotPassword(email).subscribe({
      next: () => {
        this.loading.set(false);
        this.successMessage.set('E-mail de recuperação enviado com sucesso.');
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err?.error?.message ?? 'Erro ao enviar e-mail de recuperação.');
      },
    });
  }

  onResetPassword(newPassword: string): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.resetPassword(this.token(), newPassword).subscribe({
      next: () => {
        this.loading.set(false);
        this.successMessage.set('Senha alterada com sucesso.');
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err?.error?.message ?? 'Erro ao redefinir senha.');
      },
    });
  }
}
