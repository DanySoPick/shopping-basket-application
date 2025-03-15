'use client';
import styles from "./page.module.css";
import CustomerAPIService from "./util/shoppingApi";
import { useEffect, useState } from "react";
import { ICustomer } from "./data/ICustomer";


const data = [
  { id: 1, name: "Item 1", price: "$10" },
  { id: 2, name: "Item 2", price: "$20" },
  { id: 3, name: "Item 3", price: "$30" },
];



const Home = ()  =>{

  const [customer, setCustomer] = useState<ICustomer | null>(null);

  useEffect(() => {
    const fetchCustomer = async () => {
      const customer = await CustomerAPIService.getMyCustomer();
      console.log("data",customer);
      setCustomer(customer);
    };
      fetchCustomer();
  }
  ,[]);

  return <>
  <h1>Shopping Basket</h1>
      <h2>Customer</h2>
      <div className={styles.customer}>
        <div>
          <h3>{customer?.name}</h3>
          <p>{customer?.email}</p>
          <p>Last login: {customer?.lastLogin?.toLocaleString()}</p>
        </div>
        </div>
      <table className={styles.table}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.name}</td>
              <td>{item.price}</td>
            </tr>
          ))}
        </tbody>
      </table></>;
}

export default Home;
