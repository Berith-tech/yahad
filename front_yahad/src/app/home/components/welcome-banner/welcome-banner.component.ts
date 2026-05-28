import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-welcome-banner',
  standalone: true,
  imports: [],
  templateUrl: './welcome-banner.component.html',
})
export class WelcomeBannerComponent {
  @Input() user: any;
}
