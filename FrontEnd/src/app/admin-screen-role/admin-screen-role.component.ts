
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';





import { finalize, switchMap } from 'rxjs';
import { AppScreen } from '../shared/interfaces/app-screen.interface';
import { AppScreenService } from './services/app-screen.service';
import { PermissionsService } from './services/permissions.service';
import { RoleService } from '../users/services/role.service';
import { SelectOption } from '../shared/interfaces/select-option.interface';
import { CommonModule } from '@angular/common';
import { GenericTitleComponent } from '../shared/components/generic-title/generic-title.component';
import { PrimengModule } from '../shared/modules/primeng.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PickListModule } from 'primeng/picklist';
import { DragDropModule } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-permissions',
  templateUrl: './admin-screen-role.component.html',
  providers: [MessageService, RoleService, AppScreenService, PermissionsService],
   standalone: true,
  imports: [
    CommonModule, 
    DragDropModule,


    PrimengModule,
    FontAwesomeModule,
    PickListModule

],
})
export class AdminScreenRoleComponent implements OnInit {
  // Listas maestras
  allRoles: SelectOption[] = [];
  allScreens: AppScreen[] = [];
  
  // Estado de la UI
  selectedRole: SelectOption = {
    id: '',
    text: ''
  }
  loadingRoles = true;
  loadingPermissions = false;

  // Para el PickList
  sourceScreens: AppScreen[] = []; // Pantallas no asignadas
  targetScreens: AppScreen[] = []; // Pantallas asignadas

  constructor(
    private messageService: MessageService,
    private roleService: RoleService,
    private screenService: AppScreenService,
    private permissionsService: PermissionsService
  ) {}

  ngOnInit(): void {
    this.loadInitialData();
  }


loadInitialData(): void {
  this.loadingRoles = true;

      this.roleService.getRolesDropdown().pipe(
        // 1. switchMap recibe el resultado del primer observable (roles)
        switchMap(roles => {
          // 2. Procesamos el primer resultado aquí
          this.allRoles = roles || [];
          
          // 3. Y retornamos el SIGUIENTE observable que queremos ejecutar
          return this.screenService.GetAllAppScreensAvailable2();
        }),
        // 4. finalize se ejecuta al final, sin importar si hubo éxito o error
        finalize(() => {
          this.loadingRoles = false;
        })
      ).subscribe({
        // 5. 'next' recibe el resultado del ÚLTIMO observable en la cadena (screens)
        next: screens => {
          this.allScreens = screens || [];
          this.sourceScreens = [...this.allScreens]; // Asignamos el resultado final
        },
        // 6. 'error' captura cualquier error que ocurra en CUALQUIER punto de la cadena
        error: err => {
          console.error('Error loading initial data:', err);
          this.messageService.add({ 
            severity: 'error', 
            summary: 'Error', 
            detail: 'Failed to load initial data' 
          });
        }
      });

   
  }

  onRoleChange(): void {
    if (!this.selectedRole) {
      this.sourceScreens = [...this.allScreens];
      this.targetScreens = [];
      return;
    }

    this.loadingPermissions = true;
    this.permissionsService.getPermissionsByRole(Number(this.selectedRole.id)).subscribe({
      next: assignedPermissions => {
        // Las pantallas asignadas son el 'target'
        this.targetScreens = this.allScreens.filter(screen => 
          assignedPermissions.some(p => p.fkScreen === screen.appScreenID )
        );

        // Las pantallas no asignadas son el 'source'
        const targetIds = new Set(this.targetScreens.map(s => s.appScreenID ));
        this.sourceScreens = this.allScreens.filter(s => !targetIds.has(s.appScreenID ));
        
        this.loadingPermissions = false;
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load permissions for the selected role' });
        this.loadingPermissions = false;
      }
    });
  }

  savePermissions(): void {
    if (!this.selectedRole) return;

    const screenIds = this.targetScreens.map(screen => screen.appScreenID );
    const payload = { roleId: Number(this.selectedRole.id), screenIds };

    this.permissionsService.syncPermissions(payload).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Permissions updated successfully' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error?.message || 'Failed to save permissions' });
      }
    });
  }
}