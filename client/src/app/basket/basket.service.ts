import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/products';

@Injectable({
  providedIn: 'root',
})
export class BasketService implements OnInit {
  baseURl = environment.apiUrl;

  private basketSource = new BehaviorSubject<Basket | null>(null);
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  getBasket(id: string) {
    return this.http.get<Basket>(this.baseURl + 'basket?id=' + id).subscribe({
      next: (basket: Basket) => {
        this.basketSource.next(basket);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  setBasket(basket: Basket) {
    return this.http.post<Basket>(this.baseURl + 'basket', basket).subscribe({
      next: (response: Basket) => {
        this.basketSource.next(response);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

//AutoMapper Product to BasketItem
   private mapProductItemToBasketItem(item: Product, quantity: number): BasketItem {
      return {
        id: item.id,
        productName: item.name,
        price: item.price,
        quantity: 0,
        pictureUrl: item.pictureUrl,
        brand: item.productBrand,
        type: item.productType,
      };
    }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }
     
    private createBasket(): Basket {
      const basket = new Basket();
      // we will use local storage to get id of basket againe if the computer restarts or something..
      localStorage.setItem('basket_id', basket.id);
      return basket;
    }
    
    addItemToBasket(item: Product, quantity = 1) {
      const itemToAdd: BasketItem = this.mapProductItemToBasketItem(
        item,
        quantity
      );
      const basket = this.getCurrentBasketValue() ?? this.createBasket();
      basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
      this.setBasket(basket);
    }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find((i) => i.id === itemToAdd.id);
    if (item) {
      item.quantity += quantity;
    }else{
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }
}
