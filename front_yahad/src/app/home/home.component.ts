import { Component, OnInit, inject } from '@angular/core';
import { UserService } from '../core/services/user.service';
import { WelcomeBannerComponent } from './components/welcome-banner/welcome-banner.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [WelcomeBannerComponent],
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  private userService = inject(UserService);

  user: any;

  ngOnInit(): void {
    this.userService.getProfile().subscribe({
      next: (profile) => {
        this.user = profile;
      },
    });
  }
}
