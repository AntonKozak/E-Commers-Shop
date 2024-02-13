import { NgModule } from '@angular/core';
import { OrderRoutningModule } from './order-routning.module';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { OrderDetailComponent } from './order-detail/order-detail.component';



@NgModule({
  declarations: [
    OrderComponent,
    OrderDetailComponent
  ],
  imports: [
    CommonModule,
    OrderRoutningModule
  ],
  exports:[
    CommonModule,
    OrderRoutningModule
  ]

})
export class OrderModule { }
