import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/products';
import { DeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root',
})
export class BasketService implements OnInit {
  baseURl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotalSourse= new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSourse$ = this.basketTotalSourse.asObservable();
  shipping = 0;

  constructor(private http: HttpClient,) {}

  setShippingPrice(deliveryMethod: DeliveryMethod){
    const basket = this.getCurrentBasketValue();
    this.shipping = deliveryMethod.price;
    if(basket){
      basket.deliveryMethodId = deliveryMethod.id;
      this.setBasket(basket);
    }
  }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  getBasket(id: string) {
    return this.http.get<Basket>(this.baseURl + 'basket?id=' + id).subscribe({
      next: (basket: Basket) => {
        this.basketSource.next(basket);
        this.calculateTotals();
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  setBasket(basket: Basket) {
    return this.http.post<Basket>(this.baseURl + 'basket', basket).subscribe({
      next: response => {
        this.basketSource.next(response);
        this.calculateTotals();
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
    
    addItemToBasket(item: Product | BasketItem, quantity = 1) {
      
      if(this.isProduct(item)) item = this.mapProductItemToBasketItem(item, quantity);
      const basket = this.getCurrentBasketValue() ?? this.createBasket();
      basket.items = this.addOrUpdateItem(basket.items, item, quantity);
      this.setBasket(basket);
    }

    public  removeItemFromBasket(id: number, quantity =1) {
        const basket = this.getCurrentBasketValue();
        if(!basket) return;
        const item = basket.items.find(i => i.id === id);
        if(item){
          item.quantity -= quantity;
          if(item.quantity === 0){
            basket.items = basket.items.filter(i => i.id !== id);
          }
          if(basket.items.length > 0){
            this.setBasket(basket);
        }else{
          this.deleteBasket(basket);
        }
      }
  }
    private  deleteBasket(basket: Basket) {
        return this.http.delete(this.baseURl + 'basket?id=' + basket.id).subscribe({
          next: () => {
            this.deleteLocalBasket();
          },
          error: (error) => {
            console.log(error);
          },
        });
      }

  deleteLocalBasket() {
    this.basketSource.next(null);
    this.basketTotalSourse.next(null);
    localStorage.removeItem('basket_id');
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

  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    if(!basket) return;
    
    const subtotal = basket?.items.reduce((previosValue, currentValue) => (currentValue.price * currentValue.quantity) + previosValue, 0) ?? 0;
    const total = subtotal + this.shipping;
    this.basketTotalSourse.next({shipping: this.shipping, total, subtotal});
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }

}
