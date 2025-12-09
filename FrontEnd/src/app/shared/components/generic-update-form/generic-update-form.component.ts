import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../../shared/interfaces/UsersInterfaces/user.interface';
import { GenericUpdate } from '../../interfaces/generic-update.interface';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'generic-update-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './generic-update-form.component.html',
  styleUrls: ['./generic-update-form.component.scss'],
})
export class GenericUpdateFormComponent implements OnInit {

  @Input() currentUpdate!: GenericUpdate; // Recibe el objeto a actualizar
  @Output() updateSubmitted = new EventEmitter<GenericUpdate>(); // Emite el objeto actualizado

  updateMessage: string = ''; // Mensaje de actualización
  showConfirmation = false; // Controla la visualización de la confirmación
  user!: User; // Usuario logueado

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.user = this.authService.getLoggedUser(); // Obtiene el usuario logueado desde el servicio de autenticación
  }

  confirmUpdate() {
    if (this.updateMessage !== '') {
      this.showConfirmation = true;
    } else {
      this.showError();
    }
  }

  showError() {
    setTimeout(() => {
      this.showConfirmation = false;
    }, 3000);
  }

  cancelUpdate() {
    this.showConfirmation = false;
  }

  applyUpdate() {
    this.showConfirmation = false;
    
    const updatedObject: GenericUpdate = {
      id: this.currentUpdate.id,
      updatedAt: new Date().toISOString(), // Fecha de modificación
      user: this.user, // Usuario logueado
      message: this.updateMessage, // Mensaje de actualización
    };
    
    this.updateSubmitted.emit(updatedObject); // Emite el objeto actualizado
  }

  clearForm() {
    this.updateMessage = ''; // Limpia el mensaje
    this.showConfirmation = false; // Oculta la confirmación
  }
}
