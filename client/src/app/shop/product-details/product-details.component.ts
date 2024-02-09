import { Component, OnInit, inject } from '@angular/core';
import { Product } from 'src/app/shared/models/products';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit{
  
  product?: Product;
  quantity = 1;
  quantityInBasket = 0;
  
  //ActivatedRoute to get product.id from routerlink
  constructor(private shopService: ShopService, 
    private activatedRoute: ActivatedRoute, 
    private bcService: BreadcrumbService,
    private basketService: BasketService
    ){
    this.bcService.set('@productDetails', ' ');
  }
  
  ngOnInit(): void {
    this.loadProduct();
  }
  
  loadProduct(){
    const id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    console.log(id);
    if(id){
      // Extract the product ID from the route parameters
      this.shopService.getProduct(id).subscribe({
        next: (product) => {
          this.product = product
          //comes from shop-routning to show the breadcrumb names of router links
          this.bcService.set('@productDetails', product.name);

          this.basketService.basketSource$.pipe(take(1)).subscribe({
            next: (basket) => {
              const item = basket?.items.find(i => i.id === product.id);
              // If the product is already in the basket, update the quantity in the basket.
              if(item){
                this.quantity = item.quantity;
                this.quantityInBasket = item.quantity;
              }
            }
          })
        },
        error: (error) => console.log(error)
      });
    }
  }

  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
      this.quantity--;
  }
// Updates the basket based on the selected quantity.
  updateBasket(){ 
    if(this.product)
    if(this.quantity > this.quantityInBasket){
      // Calculate the number of items to add to the basket.
      const itemToAdd = this.quantity - this.quantityInBasket;
      this.quantityInBasket += itemToAdd;
      this.basketService.addItemToBasket(this.product, itemToAdd);
    }else {
      // If the selected quantity is less than the quantity already in the basket, remove items from the basket.
      // Calculate the number of items to remove from the basket.
      const itemToRemove = this.quantityInBasket - this.quantity;
      this.quantityInBasket -= itemToRemove;
      this.basketService.removeItemFromBasket(this.product.id, itemToRemove);
    }
  }
  // Determines the text to display on the button based on the quantity in the basket.
  get buttonText(){
    // If the quantity in the basket is greater than 0, display 'Update Basket'.
    if(this.quantityInBasket > 0){
      return 'Update Basket';
    }
    // If the quantity in the basket is 0, display 'Add to Basket'.
    return 'Add to Basket';
  }

}
