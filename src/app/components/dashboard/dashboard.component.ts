import { Component } from '@angular/core';

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  products: Product[] = [
    { id: 1, name: 'Product 1', description: 'Description 1', price: 100, imageUrl: '/assets/product1.jpg' },
    { id: 2, name: 'Product 2', description: 'Description 2', price: 150, imageUrl: '/assets/product2.jpg' },
    // Add more products as needed
  ];

  addToCart(product: Product): void {
    console.log('Adding to cart:', product);
    // Implement cart logic here
  }
}
