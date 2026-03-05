import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit, AfterViewInit {

  constructor(private router: Router) { }

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

    // $('[data-widget="treeview"]').Treeview('init');

  }

}
