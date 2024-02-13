import { Component, OnInit } from '@angular/core';
import { OrderService } from './order.service';
import { Order } from '../shared/models/order';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit{
  orders: Order[] = [];

  constructor(private orderService: OrderService) {  }

  ngOnInit(): void {
    this.getOrders();
    console.log(this.orders);
  }

  getOrders() {
    this.orderService.getOrders().subscribe({
      next: (response) => {
        this.orders = response;
        console.log(this.orders);
      },
      error: (error) => console.log(error),
    });
  }

  getOrder(id: number) {
    this.orderService.getOrder(id).subscribe();
  }

}
