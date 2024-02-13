import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { RouterModule, Routes } from '@angular/router';
import { OrderDetailComponent } from './order-detail/order-detail.component';

const routes: Routes = [
  {path: '', component: OrderComponent},
  {path: ':id', component: OrderDetailComponent, data: {breadcrumb: {alias: 'OrderDetail'}}},
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class OrderRoutningModule { }
