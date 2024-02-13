import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent {
  order?: Order;

  constructor(private route: Router) { 
    const navigation = this.route.getCurrentNavigation();
    if (navigation)
    this.order = navigation && navigation.extras.state as Order;
  }
}
