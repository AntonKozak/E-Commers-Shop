import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/products';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams: ShopParams = new ShopParams();
  productCache = new Map<string, Pagination<Product[]>>();

  private http = inject(HttpClient);

  getProducts(useCache = true): Observable<Pagination<Product[]>>  {
    // Initialize productCache if caching is disabled
    if(!useCache) this.productCache = new Map();
    // Check if productCache has cached data and caching is enabled
    if(this.productCache.size > 0 && useCache) {
      // Check if cache contains data for current shopParams( mathcing key in productCache)
      if(this.productCache.has(Object.values(this.shopParams).join('-'))) {
        // Set pagination to the cached data or undefined that is we make a if statement to check if the data is cached below
        this.pagination = this.productCache.get(Object.values(this.shopParams).join('-'));
        // Return the cached data if it exists
        if (this.pagination)return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandId > 0) {params = params.append('brandId', this.shopParams.brandId);}
    if (this.shopParams.typeId > 0) {params = params.append('typeId', this.shopParams.typeId);}
    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);
    if (this.shopParams.search) 
    params = params.append('search', this.shopParams.search);

    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products', {params} ).pipe(
      map(response => {
        // Cache the response
        this.productCache.set(Object.values(this.shopParams).join('-'), response);
        this.pagination = response;

        return response;
      })
    )
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    
    const product = [...this.productCache.values()]
    // .reduce is used to flatten the array of arrays into a single array
    .reduce((accumulator, paginationResult) => {
      // Find the product with the id in the paginationResult data and return it
      return {...accumulator, ...paginationResult.data.find(p => p.id === id)}
    }, {} as Product)

    //return as Observable<Product>;
    //Check if the product is not empty
    if (Object.keys(product).length !== 0) return of(product)

    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if(this.brands.length > 0) return of(this.brands)

    return this.http.get<Brand[]>(this.baseUrl + 'products/brands').pipe(
      map(brands => this.brands = brands));
  }

  getTypes() {
    if(this.types.length > 0) return of(this.types)

    return this.http.get<Type[]>(this.baseUrl + 'products/types').pipe(
      map(types => this.types = types));;
  }

}
