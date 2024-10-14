import { Component,OnInit, Output, EventEmitter, Input } from '@angular/core'
import { MatDivider } from '@angular/material/divider';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';

import { MatNavList, MatListItem } from '@angular/material/list';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrl: './menu.component.scss',
    standalone: true,
    imports: [MatNavList, MatIconButton, MatIcon, MatListItem, RouterLink, RouterLinkActive, MatDivider]
})
export class MenuComponent implements OnInit {

  @Input() isOpened?: boolean;
  @Output() sidenavToggle = new EventEmitter<void>();

  constructor() {}

  ngOnInit() {}

  onToggleSidenav() {
    this.sidenavToggle.emit();
  }

}