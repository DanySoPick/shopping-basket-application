export interface IProduct {
    id: number;
    name: string;
    price: number;
  }

  export interface IDiscount {
    id: number;
    productId: number;
    discountType: string;
    discountValue: number;
    requiredProductId : number;
    requiredQuantity: number;
  }

  export interface IProductBundle {
    products: IProduct[];
    discounts: IDiscount[];
  }

