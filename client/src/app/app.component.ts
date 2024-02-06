import { Component, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/products';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  
  products: Product[] = [];

  private http = inject(HttpClient);

  ngOnInit(): void {
    this.getValues();
  }

  getValues() {
    this.http.get<Pagination<Product[]>>('https://localhost:5001/api/Products').subscribe({
      next: response => this.products = response.data,
      error: (error) => console.log(error),
      complete: () => {
        console.log('Complete');
      },
    });
  }
}
