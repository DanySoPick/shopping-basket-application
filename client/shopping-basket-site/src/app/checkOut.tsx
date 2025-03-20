import { IDiscount, IProduct } from "./data/IProduct";
import { IBasket } from "./data/Ibasket";
import { ICustomer } from "./data/ICustomer";
import styles from "./page.module.css";
import { useEffect, useState } from "react";

interface CheckOutProps {
  products: IProduct[];
  selectedProducts: IBasket[];
  discounts: IDiscount[];
  customer: ICustomer | null;
  onCancel: () => void;
}

const CheckOut = ({
    products,
    selectedProducts,
    discounts,
    customer,
    onCancel,
  }: CheckOutProps) => {
    console.debug("CheckOut", selectedProducts, discounts, customer);
  
    const [totalCost, setTotalCost] = useState(0);
    const [discountedCost, setDiscountedCost] = useState(0);
  
    // Calculate total cost without discounts
    useEffect(() => {
      const total = selectedProducts.reduce((sum, product) => sum + product.cost, 0);
      setTotalCost(parseFloat(total.toFixed(2)));
    }, [selectedProducts]);
  
    // Calculate discounted cost
    useEffect(() => {
      const calculateDiscountedCost = () => {
        const discountedTotal = selectedProducts.reduce((sum, product) => {
          const discount = discounts.find((disc) => disc.productId === product.productId);
          if (discount) {
            if (discount.discountType === "MULTI_BUY") {
              const requiredProduct = selectedProducts.find(
                (selectedProduct) =>
                  selectedProduct.productId === discount.requiredProductId
              );
              if (
                requiredProduct &&
                requiredProduct.quantaty >= discount.requiredQuantity
              ) {
                return sum + (product.cost - discount.discountValue); // Apply multi-buy discount
              }
            } else if (discount.discountType === "PERCENTAGE") {
              return sum + product.cost * (1 - discount.discountValue / 100); // Apply percentage discount
            } else {
              return sum + (product.cost - discount.discountValue); // Apply flat discount
            }
          }
          return sum + product.cost; // No discount applied
        }, 0);
        setDiscountedCost(parseFloat(discountedTotal.toFixed(2)));
      };
  
      calculateDiscountedCost();
    }, [selectedProducts, discounts]);
  
    const getProductNameFromDiscount = (productId: number) => {
      const thisProduct = products.find((product) => product.id === productId);
      if (thisProduct) {
        return thisProduct.name;
      }
      return "Unknown product";
    };
  
    return (
      <div className={styles.checkoutContainer}>
        <h1>CheckOut</h1>
        {customer && (
          <div className={styles.customerInfo}>
            <h2>Customer Information</h2>
            <p>Name: {customer.name}</p>
            <p>Email: {customer.email}</p>
          </div>
        )}
        <div className={styles.productList}>
          <h2>Selected Products</h2>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Product ID</th>
                <th>Quantity</th>
                <th>Cost</th>
              </tr>
            </thead>
            <tbody>
              {selectedProducts.map((product) => (
                <tr key={product.productId}>
                  <td>{product.productId}</td>
                  <td>{product.quantaty}</td>
                  <td>${product.cost.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className={styles.discountList}>
          <h2>Discounts</h2>
          <ul>
            {discounts.map((discount) => (
              <li key={discount.id}>
                {discount.discountType === "MULTI_BUY" ? (
                  <>
                    {discount.discountName} applied to product{" "}
                    {getProductNameFromDiscount(discount.requiredProductId)}. Get for free:{" "}
                    {getProductNameFromDiscount(discount.productId)}.
                  </>
                ) : discount.discountType === "PERCENTAGE" ? (
                  <>
                    {discount.discountName} applied to product{" "}
                    {getProductNameFromDiscount(discount.productId)} with a discount of{" "}
                    {discount.discountValue}%.
                  </>
                ) : (
                  <>
                    {discount.discountName} applied to product{" "}
                    {getProductNameFromDiscount(discount.productId)} with a discount of{" "}
                    ${discount.discountValue}.
                  </>
                )}
              </li>
            ))}
          </ul>
        </div>
        <div className={styles.totalCost}>
          <h2>Total Cost: ${(totalCost-discountedCost).toFixed(2)}</h2>
          <h2>Discounted Cost: ${discountedCost.toFixed(2)}</h2>
        </div>
        <button className={styles.checkoutButton}>Buy</button>
        <button onClick={onCancel} className={styles.cancelButton}>
          Cancel
        </button>
      </div>
    );
  };
  
  export default CheckOut;
