import { Product } from "./product.inteface";

export interface Purchase {
    id?: number;
    userId?: number;
    products?: Array<Product>;
    purchaseDate?: Date;
}