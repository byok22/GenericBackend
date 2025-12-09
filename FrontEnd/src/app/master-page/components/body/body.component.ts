import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { IBody } from '../../builder/master-page.interface';

@Component({
  selector: 'body-app',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './body.component.html',
  styleUrl: './body.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BodyComponent {

  @Input() body: IBody = {
    collapsed :false,
    screenWidth: 0
  }

  // get the style class for the body element based on the screen width and the collapsed state of the sidebar
  getBodyClass(): string {
    let styleClass = '';
    if(this.body.collapsed && this.body.screenWidth > 768) {
      styleClass = 'body-trimmed';
    } else if(this.body.collapsed && this.body.screenWidth <= 768 && this.body.screenWidth > 0) {
      styleClass = 'body-md-screen'
    }
    return styleClass;
  }
  
 }
