"use client";
import styles from "./page.module.css";
import CustomerAPIService from "./util/shoppingApi";
import { ChangeEvent, useEffect, useState } from "react";
import { ICustomer } from "./data/ICustomer";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { IBasket } from "./data/Ibasket";
import CheckOut from "./checkOut";
import { IDiscount, IProduct } from "./data/IProduct";

const defaultData: IBasket[] = [
  {
    productId: 0,
    quantaty: 1,
    cost: 0,
  },
];

const Home = () => {
  const [data, setData] = useState(() => [...defaultData]);
  const [customer, setCustomer] = useState<ICustomer | null>(null);
  const [products, setProducts] = useState<IProduct[] | []>([
    { id: 0, name: "No product", price: 0 },
  ]);
  const [discounts, setDiscounts] = useState<IDiscount[] | []>([]);
  const [multiDiscount, setMultiDiscount] = useState<IDiscount[] | []>([]);
  const [isChekout, setIsChekout] = useState(false);

  useEffect(() => {
    const fetchCustomer = async () => {
      const customer = await CustomerAPIService.getMyCustomer();
      console.log("data", customer);
      setCustomer(customer);
    };
    const fetchProducts = async () => {
      const productBundle = await CustomerAPIService.getProducts();

      // Ensure `product.values` is an array or fallback to an empty array
      const products = productBundle?.products || [];
      const discounts = productBundle?.discounts || [];

      setProducts((prevProducts) => [...prevProducts, ...products]);

      if (discounts.length > 0) {
        const multiDiscount = discounts.filter(
          (discount) => discount.discountType === "multi"
        );
        setMultiDiscount(multiDiscount);
        setDiscounts(discounts);
      }
    };

    fetchCustomer();
    fetchProducts();
  }, []);

  type Option = {
    label: string;
    value: number | string;
  };

  const columnHelper = createColumnHelper<IBasket>();

  const TableCell = ({
    getValue,
    row,
    column,
    table,
  }: {
    getValue: () => any;
    row: any;
    column: any;
    table: any;
  }) => {
    const initialValue = getValue();
    const columnMeta = column.columnDef.meta;
    const tableMeta = table.options.meta;
    const [value, setValue] = useState(initialValue);

    useEffect(() => {
      setValue(initialValue);
    }, [initialValue]);

    const onBlur = () => {
      table.options.meta?.updateData(row.index, column.id, value);
    };

    const onSelectChange = (e: ChangeEvent<HTMLSelectElement>) => {
      const selectedProductId = parseInt(e.target.value, 10);
      setValue(selectedProductId);

      // Find the selected product
      const selectedProduct = products.find(
        (product) => product.id === selectedProductId
      );

      if (selectedProduct) {
        // Update the productId and cost in the row
        tableMeta?.updateData(row.index, "productId", selectedProductId);
        const quantity = row.getValue("quantaty") || 1; // Default to 1 if quantity is not set
        const cost = (selectedProduct.price * quantity).toFixed(2); // Ensure 2 decimal places
        tableMeta?.updateData(row.index, "cost", parseFloat(cost)); // do parsFloat becayse toFixed convert to string
      }
    };

    const onQuantityChange = (e: ChangeEvent<HTMLInputElement>) => {
      const newQuantity = parseInt(e.target.value, 10) || 0;
      setValue(newQuantity);

      // Update the quantity and recalculate the cost
      tableMeta?.updateData(row.index, "quantaty", newQuantity);
      const productId = row.getValue("productId");
      const selectedProduct = products.find(
        (product) => product.id === productId
      );

      if (selectedProduct) {
        // Check if there is a discount for the selected product
        const currentDiscounts = discounts.find(
          (discount) => discount.requiredProductId === selectedProduct.id
        );

        if (
          currentDiscounts?.discountType === "MULTI_BUY" &&
          selectedProduct.id === currentDiscounts.requiredProductId
        ) {
          const requiredQuantity = currentDiscounts.requiredQuantity;
    
          const offerProductId = currentDiscounts.productId;
          if (newQuantity >= requiredQuantity) {
            const offerProduct = products.find((product) => product.id === offerProductId);
    
            if (offerProduct) {
              // Check if the free offer product is already in the table
              const isOfferProductAdded = data.some(
                (row) => row.productId === offerProduct.id && row.cost === 0
              );

              if (!isOfferProductAdded) {
                // Add the free offer product to the table
                const newRow: IBasket = {
                  productId: offerProduct.id,
                  quantaty: 1, // Free product quantity is 1
                  cost: 0, // Free product has no cost
                };
    
              setData((prevData) => [...prevData, newRow]);
            }
          }
          }
          else {
            // Remove the free offer product if the condition is no longer met
            setData((prevData) =>
              prevData.filter((row) => row.productId !== offerProductId || row.cost !== 0)
            );
          }
        }
        const cost = (selectedProduct.price * newQuantity).toFixed(2); // Ensure 2 decimal places
        tableMeta?.updateData(row.index, "cost", parseFloat(cost)); // do parsFloat becayse toFixed convert to string
      }
    };

    return columnMeta?.type === "select" ? (
      <select onChange={onSelectChange} value={value}>
        {columnMeta?.options?.map((option: IProduct) => (
          <option key={option.id} value={option.id}>
            {option.name}
          </option>
        ))}
      </select>
    ) : column.id === "quantaty" ? (
      <input
        value={value}
        onChange={onQuantityChange}
        onBlur={onBlur}
        type="number"
        min="1"
      />
    ) : (
      <input
        value={value}
        onChange={(e) => setValue(e.target.value)}
        onBlur={onBlur}
        type={columnMeta?.type || "text"}
      />
    );
  };

  const columns = [
    columnHelper.accessor("productId", {
      header: "Product",
      cell: TableCell,
      meta: {
        type: "select",
        options: products,
      },
    }),
    columnHelper.accessor("quantaty", {
      header: "Quantity",
      cell: TableCell,
      meta: {
        type: "number",
      },
    }),
    columnHelper.accessor("cost", {
      header: "Cost",
      meta: {
        type: "number",
      },
    }),
  ];

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    meta: {
      updateData: (rowIndex: number, columnId: string, value: string) => {
        setData((old) =>
          old.map((row, index) => {
            if (index === rowIndex) {
              return {
                ...old[rowIndex],
                [columnId]: value,
              };
            }
            return row;
          })
        );
      },
    },
  });

  const addRow = () => {
    const newRow: IBasket = {
      productId: 0, // Default product ID
      quantaty: 1, // Default quantity, even if its 1 the cost is 0 so it will not affect the total cost
      cost: 0, // Default cost
    };
    setData((prevData) => [...prevData, newRow]);
  };

  const checkout = () => {
    setIsChekout(true);
    const totalCost = data.reduce((sum, row) => sum + row.cost, 0).toFixed(2);
    //alert(`Total cost: $${totalCost}`); //testing
  };

  return (
    <>
      <div className={styles.container}>
        <h1>Shopping Basket</h1>
        <div className={styles.addButtonContainer}>
          <button onClick={addRow} className={styles.addButton}>
            Add Row
          </button>
          <button onClick={checkout} className={styles.checkoutButton}>
            Checkout
          </button>
        </div>
        <table className={styles.table}>
          <thead>
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.map((row) => (
              <tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {isChekout && (
        <CheckOut
          selectedProducts={data}
          discounts={discounts}
          customer={customer}
          onCancel={() => setIsChekout(false)}
        />
      )}
    </>
  );
};

export default Home;
