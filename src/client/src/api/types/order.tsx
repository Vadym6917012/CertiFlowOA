export interface Order {
  orderId: number;
  userId: string;
  documentId: number;
  purpose: string;
  format: string;
  createdDate: string;
  completedDate: string | null;
  userName: string;
  documentStatus: string;
  documentTypeName: string;
}