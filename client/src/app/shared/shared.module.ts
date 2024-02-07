import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';




@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot()//to load as singelton (one instanse of the module for the entire app)
  ],
  exports: [
    PaginationModule
  ]
})
export class SharedModule { }
