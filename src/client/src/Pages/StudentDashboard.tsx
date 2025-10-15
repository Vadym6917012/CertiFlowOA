import { useCallback, useEffect, useState } from "react";
import RequestForm from "../components/RequestForm";
import RequestList from "../components/RequestList";
import apiClient from "../api/apiClient";
import { Order } from "../api/types/order";

export default function StudentDashboard() {
  const [showForm, setShowForm] = useState(false);
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchOrders = useCallback(async () => {
    try {
      setLoading(true);
      const response = await apiClient.get<Order[]>("/Orders/get-my-orders");
      setOrders(response.data);
    } catch (error) {
      console.error("Error fetching orders:", error);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchOrders();
  }, [fetchOrders]);

  return (
      <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <button
          className="btn btn-outline-primary d-md-none"
          type="button"
          data-bs-toggle="offcanvas"
          data-bs-target="#mobileSidebar"
        >
          ☰ Меню
        </button>

        <button
          onClick={() => setShowForm(!showForm)}
          className={`btn ${showForm ? "btn-outline-danger" : "btn-primary"}`}
        >
          {showForm ? "Закрити форму" : "➕ Замовити довідку"}
        </button>
      </div>

      {showForm && <RequestForm onSuccess={fetchOrders} />}

      <section className="mt-4">
        <h2 className="h5 mb-3">Мої замовлення</h2>
        <RequestList orders={orders} loading={loading} />
      </section>
    </div>
  );
}
