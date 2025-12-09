import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';

// Definimos las interfaces y enums aquí para evitar errores de importación
export enum NavStatus {
  Collapsed = 'Collapsed',
  Expand = 'Expand',
  Pinned = 'Pinned'
}

export interface NavItem {
  name: string;
  href: string;
  icon?: string;
  childrens?: NavItem[];
  expanded?: boolean;
  external?: boolean;
}

@Component({
  selector: 'app-sublevel-menu',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({opacity: 0}),
        animate('1200ms',
          style({opacity: 1})
        )
      ]),
      transition(':leave', [
        style({opacity: 1}),
        animate('300ms',
          style({opacity: 0})
        )
      ])
    ]),
    trigger('submenu', [
      state('hidden', style({
        height: '0',
        overflow: 'hidden'
      })),
      state('visible', style({
        height: '*'
      })),
      transition('visible <=> hidden', [
        style({ overflow: 'hidden' }),
        animate('{{transitionParams}}', style({ height: '{{height}}' }))
      ]),
      transition('void => *', animate(0)),
    ]),
  ],
  template: `
    <ul *ngIf="shouldDisplaySubmenu" [@submenu]="submenuAnimationParams" class="sublevel-nav">
      <ng-container *ngFor="let child of item.childrens">
        <li class="sublevel-nav-item" [ngClass]="{'sublevel-line': hasChildren(child)}">
          <ng-container *ngIf="hasChildren(child); else leafTemplate">
            <a class="sublevel-nav-link" (click)="handleClick(child)" [ngClass]="getActiveClass(child)">
              <i class="sublevel-link-icon" [ngClass]="child.icon || 'fa fa-circle'"></i>
              <span class="sublevel-link-text" @fadeInOut *ngIf="isExpanded">{{ child.name }}</span>
              <i *ngIf="isExpanded" class="menu-collapse-icon" [ngClass]="child.expanded ? 'fa fa-angle-down' : 'fa fa-angle-right'"></i>
            </a>
            <app-sublevel-menu
              *ngIf="child.expanded"
              [item]="child"
              [navStatus]="navStatus"
              [multiple]="multiple"
            ></app-sublevel-menu>
          </ng-container>
          <ng-template #leafTemplate>
            <a
              class="sublevel-nav-link"
              [routerLink]="child.external ? null : [child.href]"
              [href]="child.external ? child.href : null"
              [target]="child.external ? '_blank' : undefined"
              routerLinkActive="active"
              [routerLinkActiveOptions]="{ exact: true }"
            >
              <i class="sublevel-link-icon" [ngClass]="child.icon || 'fa fa-circle'"></i>
              <span class="sublevel-link-text" @fadeInOut *ngIf="isExpanded">{{ child.name }}</span>
            </a>
          </ng-template>
        </li>
      </ng-container>
    </ul>
  `,
  styleUrl: './../sidenav.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SublevelMenuComponent {
  @Input() item: NavItem = { href: '', name: '', childrens: [] };
  @Input() navStatus: NavStatus = NavStatus.Collapsed;
  @Input() expanded: boolean | undefined;
  @Input() multiple: boolean = false;

  constructor(public router: Router) {}

  get shouldDisplaySubmenu(): boolean {
    if(this.navStatus !== 'Collapsed' && this.item.childrens && this.item.childrens.length > 0){
      return true;
    }else{
      return false;
    }
  }

  get submenuAnimationParams(): any {
    return this.expanded
      ? { value: 'visible', params: { transitionParams: '500ms cubic-bezier(0.86, 0, 0.07, 1)', height: '*' } }
      : { value: 'hidden', params: { transitionParams: '500ms cubic-bezier(0.86, 0, 0.07, 1)', height: '0' } };
  }

  get isExpanded(): boolean {
    return this.navStatus?.toString() !== 'Collapsed';
  }

  hasChildren(item: NavItem): boolean {
    return item.childrens && item.childrens.length > 0 ? true : false;
  }

  handleClick(item: NavItem): void {
    if (!this.multiple && this.item.childrens) {
      this.item.childrens.forEach(modelItem => {
        if (item !== modelItem && modelItem.expanded) {
          modelItem.expanded = false;
        }
      });
    }
    item.expanded = !item.expanded;
  }

  getActiveClass(item: NavItem): string {
    return item.expanded && this.router.url.includes(item.href) ? 'active-sublevel' : '';
  }
}