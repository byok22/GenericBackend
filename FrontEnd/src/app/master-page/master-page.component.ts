import { CommonModule } from '@angular/common';
import {  Component, OnInit, signal  } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { IBody, IMasterPage } from './builder/master-page.interface';
import { MasterPageConcretBuilder } from './builder/master-page-concret-builder';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';


@Component({
  selector: 'app-master-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    HeaderComponent,
    FooterComponent,
    //ProyectsComponent,
    SidenavComponent,
],
  templateUrl: './master-page.component.html',
  styleUrl: './master-page.component.css'
})
export class MasterPageComponent implements OnInit  {

  //Page Config

 
   //#region Master Page Declarations
   master: IMasterPage = {
    body: {
        collapsed:false,
        screenWidth:0
    }
  };
  //Body
  body: IBody = {
  collapsed:true,
  screenWidth:0
  };
  collapsed: boolean = false;
  screenWidth: number = 0;

  //#endregion

  
  constructor(
  ) {
    
  }

  ngOnInit(): void{
    this.ConfigMaster();
  }
  ConfigMaster(){
    this.ConfigBody();
  }
  ConfigBody(){
    this.body.collapsed = this.collapsed;
    this.body.screenWidth = this.screenWidth;
    const builder = new MasterPageConcretBuilder();
    builder.setBody(this.body);
    this.master = builder.Generate();    
  }
  
}