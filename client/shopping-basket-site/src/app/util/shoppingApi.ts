import { ICustomer } from "../data/ICustomer";
import { IProductBundle } from "../data/IProduct";
import envVarConfig from "../env/envVarConfig";

const apiPath = envVarConfig.apiUrl;

export class CustomerAPIService {
    public static async getMyCustomer(): Promise<ICustomer | null> {
        
        console.log("apiPath",`${apiPath}/api/Basket/user1@example.com`);
        const headers = {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
          };

      const response = await fetch(`${apiPath}/api/Basket/user1@example.com`,{headers});
      if (response.ok) {
        const data = await response.json();
        return data;
      } else {
        console.log("Failed to fetch customer:", response.status);
        return null;
      }
    }

    public static async getProducts(): Promise<IProductBundle | null> {

      const headers = {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*",
      };

      const response = await fetch(`${apiPath}/products`,{headers});
      if (response.ok) {
        const data = await response.json();
        return data;
      } else {
        console.log("Failed to fetch products:", response.status);
        return null;
      }
    }
  }
  
  export default CustomerAPIService;