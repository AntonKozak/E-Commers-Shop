import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/products';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
@Input() product?: Product;

  constructor(private basketService: BasketService) { }

  addItemToBasket() {
    //like this customers can add items to the basket even if they are clicking on the product item
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
