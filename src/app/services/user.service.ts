import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Product } from '../interfaces/product.interface';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getAllProducts() {
    return this.http.get(environment.userManagement.baseUrl + 'products');
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(`${environment.userManagement.baseUrl}products`, product);
  }
}