import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/products';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  private http = inject(HttpClient);

  getProducts() {
    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'Products');
  }
}
