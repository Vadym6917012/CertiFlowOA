import { Order } from "./order";

export interface OrderResponse {
  orders: Order[];
  totalCount: number;
}
