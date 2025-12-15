import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DeleteToken, GetToken } from '../../../shared/functions/localstorage';
import { Router } from '@angular/router';
import { PrimengModule } from '../../../shared/modules/primeng.module';
import { JWT } from '../../../shared/interfaces/JWTModels';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent implements OnInit {

  userID: string = "";
  userInfo: JWT = { NTUser: '', Role: '' } as JWT;
  showUserDialog: boolean = false;
  userMenuItems: MenuItem[] = [];
  selectedSiteName: string = '';

  constructor(
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {
    const token = GetToken('token');
    if (token) {
      this.userInfo = token;
      // si NTUser viene vacío podría poner GuestView
      this.userID = this.userInfo?.NTUser ? `JABIL/${this.userInfo.NTUser}` : 'GuestView';
    } else {
      this.userInfo = { NTUser: '', Role: '' } as JWT;
      this.userID = 'GuestView';
    }

    // Leer sitio seleccionado desde localStorage (si existe)
    try {
      const siteJson = localStorage.getItem('selectedSite');
      if (siteJson) {
        const site = JSON.parse(siteJson);
        this.selectedSiteName = site?.siteName || '';
      }
    } catch (err) {
      console.error('Error parsing selectedSite from localStorage', err);
    }
  }

  ngOnInit() {
    document.addEventListener('userLoggedIn', (event: any) => {
      const detail = event?.detail ?? '';
      const nt = detail ? detail : '';
      const newUserID = nt ? (detail.startsWith('JABIL/') ? detail : `JABIL/${detail}`) : 'GuestView';
      this.userID = newUserID;
      localStorage.setItem('userID', newUserID);
   
      this.cdr.markForCheck();
    });

    // Escuchar cambios de sitio si alguna parte de la app emite el evento
    document.addEventListener('siteChanged', (event: any) => {
      const site = event?.detail ?? null;
      if (site) {
        this.selectedSiteName = site.siteName || '';
        this.cdr.markForCheck();
      }
    });

    this.initializeUserMenuItems();
  }

  private initializeUserMenuItems() {
    this.userMenuItems = [
      {
        label: 'Profile',
        icon: 'pi pi-user',
        command: () => this.openUserInfo()
      },
      {
        label: 'Logout',
        icon: 'pi pi-sign-out',
        command: () => this.handleLogOut()
      }
    ];
  }

  isLoggedIn(): boolean {
    // consider logged in only if NTUser tiene texto
    return !!(this.userInfo && this.userInfo.NTUser && this.userInfo.NTUser.trim().length > 0);
  }

  onUserButtonClick(menu: any, event: any) {
    if (this.isLoggedIn()) {
      menu.toggle(event);
    } else {
      this.goToLogin();
    }
  }

  goToLogin() {
    this.handleLogIn();
  }

  private handleLogIn() {
    DeleteToken('token');
    this.router.navigate(['/login']);
  }

  handleLogOut() {
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
    DeleteToken('token');
    sessionStorage.removeItem('userID');
    localStorage.clear();

    this.userInfo = { NTUser: '', Role: '' } as JWT;
    this.userID = 'GuestView';
    this.cdr.markForCheck();
    this.router.navigate(['/login']);
  }

  openUserInfo() {
    this.showUserDialog = true;
  }
}
