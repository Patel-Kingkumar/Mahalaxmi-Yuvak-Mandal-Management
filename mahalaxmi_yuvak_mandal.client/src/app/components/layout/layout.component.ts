import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

declare var $: any;

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit, AfterViewInit {

  constructor(private auth: AuthService, private router: Router) { }

  onLogout() {
    const confirmLogout = confirm('Are you sure you want to logout?');

    if (confirmLogout) {
      this.auth.logout();
      this.router.navigate(['/login']);
    }
  }



  ngOnInit() {

    // this.router.events.subscribe(event => {

    //   if (event instanceof NavigationEnd) {

    //     setTimeout(() => {
    //       $('[data-widget="treeview"]').Treeview('init');
    //     });

    //   }

    // });

  }

  ngAfterViewInit(): void {

    $('[data-widget="treeview"]').Treeview('init');
    // $('[data-widget="treeview"]').Treeview('init');

  }

}
