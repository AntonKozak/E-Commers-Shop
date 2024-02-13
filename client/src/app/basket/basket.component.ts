import { Component, inject } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent {


public basketServise = inject(BasketService);

  incrementQuantity(item: BasketItem) {
    this.basketServise.addItemToBasket(item);
  }

  removeItem(event: {id: number, quantity: number}) {
    this.basketServise.removeItemFromBasket(event.id, event.quantity);
  }

}
