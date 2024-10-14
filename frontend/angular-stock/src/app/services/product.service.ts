import { Injectable, inject } from '@angular/core'

// Import CookieService
import { CookieService } from 'ngx-cookie-service'

// Import HttpClient and HttpHeaders
import { HttpClient, HttpHeaders } from '@angular/common/http'

// Import Observable
import { Observable } from 'rxjs'

// Import environment
import { environment } from '../../environments/environment'
import { ProductCreateUpdateModel, ProductModel } from '../models/product.model'

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiURL = environment.dotnet_api_url
  private http = inject(HttpClient)
  private cookieService = inject(CookieService)

  // Read token from cookie
  token = this.cookieService.get("LoggedInToken") || ""

  // Header for GET, DELETE
  httpOptions  = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer '+ this.token
    })
  }

  // Header for POST, PUT
  httpOptionsPost  = {
    headers: new HttpHeaders({
      'Authorization': 'Bearer '+ this.token
    })
  }

  // Get All Products by parameter (page: number, limit: number, selectedCategory: string, searchQuery: string)
  getAllProducts(
    page: number, 
    limit: number, 
    selectedCategory: string, 
    searchQuery: string
  ): Observable<ProductModel>
  {
    let url = this.apiURL + 'Product?page=' + page + '&limit=' + limit

    if (selectedCategory) {
      url += '&selectedCategory=' + selectedCategory
    }

    if (searchQuery) {
      url += '&searchQuery=' + searchQuery
    }

    return this.http.get<ProductModel>(
      url, 
      this.httpOptions
    )
  }

  // Get Product By ID
  getProductById(id: number): Observable<ProductModel> {
    return this.http.get<ProductModel>(
      this.apiURL + 'Product/' + id, 
      this.httpOptions
    )
  }

  // Create Product
  createProduct(product: ProductCreateUpdateModel): Observable<ProductCreateUpdateModel> {
    return this.http.post<ProductCreateUpdateModel>(
      this.apiURL + 'Product', 
      product, 
      this.httpOptionsPost
    )
  }

  // Update Product
  updateProduct(id: number, product: ProductCreateUpdateModel): Observable<ProductCreateUpdateModel> {
    return this.http.put<ProductCreateUpdateModel>(
      this.apiURL + 'Product/' + id,
      product, 
      this.httpOptionsPost
    )
  }

  // Delete Product
  deleteProduct(id: number): Observable<unknown> {
    return this.http.delete<unknown>(
      this.apiURL + 'Product/' + id, 
      this.httpOptions
    )
  }

}
