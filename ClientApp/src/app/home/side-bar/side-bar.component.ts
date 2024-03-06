import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.scss'
})
export class SideBarComponent {
  isSidebarToggled = true;
 
  constructor(private router: Router) { }

  ngOnInit() {

  }

  toggleSidebar() {
    this.isSidebarToggled = !this.isSidebarToggled;
  }

  toggleSubMenu(event: MouseEvent) {
    const target = event.currentTarget as HTMLElement;
    const submenu = target.nextElementSibling as HTMLElement;

    if (submenu.style.display === 'block') {
      submenu.style.display = 'none';
      target.parentElement?.classList.remove('active');
    } else {
      submenu.style.display = 'block';
      target.parentElement?.classList.add('active');
    }
  }

  navigateTo(route: string) {
    this.router.navigate(['/dashboard']);
  }

}
