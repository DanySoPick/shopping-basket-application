import { IDiscount, IProduct } from "./data/IProduct";
import { IBasket } from "./data/Ibasket";
import { ICustomer } from "./data/ICustomer";
import styles from "./page.module.css";

interface CheckOutProps {
    selectedProducts: IBasket[];
    discounts: IDiscount[];
    customer: ICustomer | null;
    onCancel: () => void;
  }

  const CheckOut = ({ selectedProducts, discounts, customer,onCancel }: CheckOutProps) => {

    console.debug("CheckOut", selectedProducts, discounts, customer);

    return (
        <div>
            <h1>CheckOut</h1>
            <button className={styles.checkoutButton}>Buy</button>
            <button onClick={onCancel} className={styles.cancelButton}>Cancel</button>
        </div>
    );
};

export default CheckOut;
