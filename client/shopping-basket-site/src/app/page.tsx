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
import { table } from "console";

const defaultData: IBasket[] = [
  {
    productId: 1,
    quantaty: 1,
    cost: 100,
  },
  {
    productId: 2,
    quantaty: 2,
    cost: 200,
  },
];

type Option = {
  label: string;
  value: number | string;
};

const defaultProducts: Option[] = [
  {
    label: "Milk",
    value: 1,
  },
  {
    label: "Bread",
    value: 2,
  },
];

const Home = () => {
  const [data, setData] = useState(() => [...defaultData]);
  const [customer, setCustomer] = useState<ICustomer | null>(null);

  useEffect(() => {
    const fetchCustomer = async () => {
      const customer = await CustomerAPIService.getMyCustomer();
      console.log("data", customer);
      setCustomer(customer);
    };
    fetchCustomer();
  }, []);

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
      setValue(e.target.value);
      tableMeta?.updateData(row.index, column.id, e.target.value);
    };

    return columnMeta?.type === "select" ? (
      <select onChange={onSelectChange} value={initialValue}>
        {columnMeta?.options?.map((option: Option) => (
          <option key={option.value} value={option.value}>{option.label}</option>
        ))}
      </select>
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
      options: defaultProducts,
      },
    }),
    columnHelper.accessor("quantaty", {
      header: "Quantaty",
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

  return (
    <>
      <div className={styles.container}>
        <h1>Shopping Basket</h1>
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
    </>
  );
};

export default Home;
