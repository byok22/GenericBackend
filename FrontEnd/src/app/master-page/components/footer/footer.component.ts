import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

//import { ConfigService } from '../../../../core/services/config.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
   imports: [
    CommonModule,
    
],
  styleUrls: ['./footer.component.scss'],
  //providers: [ConfigService]
})
export class FooterComponent {
  status:string = 'development';
  version:string = '1.0.1';
  //constructor(private configService: ConfigService) {
  //  this.configService.getConfig().subscribe((data) => {
  //    this.status = data.environment;
  //    this.version = data.version;
  //  });
  //}
  @Input() collapsed: boolean = false;
  @Input() screenWidth: number = 0;
  // get the style class for the footer element based on the screen width and the collapsed state of the sidebar
  getFooterClass():string {
    let styleClass ='';
    if(this.collapsed && this.screenWidth > 768){
      styleClass = 'footer-trimmed';
    }
    else{
      styleClass = 'footer-md-screen';
    }
    return styleClass;
    }
}
