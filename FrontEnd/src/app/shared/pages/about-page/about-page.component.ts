import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { VersionService } from '../../services/version.service';


@Component({
  selector: 'app-about-page',
  standalone:true,
  templateUrl: './about-page.component.html',
  styleUrls: ['./about-page.component.scss']
})
export class AboutPageComponent implements OnInit {
  
  /**
   *
   */
  constructor( private versionService: VersionService) {
   
    
  }
  version:string =''

  ngOnInit(): void {
    this.version = this.versionService.get();
  }

 }
