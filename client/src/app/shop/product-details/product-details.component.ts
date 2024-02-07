import { Component, OnInit, inject } from '@angular/core';
import { Product } from 'src/app/shared/models/products';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit{
  
  product?: Product;
  
  //ActivatedRoute to get product.id from routerlink
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, private bcService: BreadcrumbService ){}
  
  ngOnInit(): void {
    this.loadProduct();
  }
  
  loadProduct(){
    const id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    console.log(id);
    if(id){
      this.shopService.getProduct(id).subscribe({
        next: (product) => {
          this.product = product
          //comes from shop-routning to show the breadcrumb names of router links
          this.bcService.set('@productDetails', product.name);
        },
        error: (error) => console.log(error)
      });
    }
  }
}
