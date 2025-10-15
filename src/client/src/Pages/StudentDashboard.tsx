import { useCallback, useEffect, useState } from "react";
import RequestForm from "../components/RequestForm";
import RequestList from "../components/RequestList";
import Sidebar from "../components/Sidebar";
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
      console.log("Orders fetched:", response.data);
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
    <>
      <div className="container-fluid">
        <div className="row">
          <Sidebar />

          <Sidebar isOffcanvas={true} />
          <main
            className="dashboard main-bg-gray py-4 col-md-9 ms-sm-auto col-lg-10 px-md-4 min-vh-100"
            id="dashboard-container"
          >
            <div className="container">
              <div className="dashboard-header mb-4 d-flex justify-content-between">
                <button
                  className="btn btn-outline-primary mb-3 d-md-none"
                  type="button"
                  data-bs-toggle="offcanvas"
                  data-bs-target="#mobileSidebar"
                  aria-controls="mobileSidebar"
                >
                  ☰ Меню
                </button>
                <a
                  onClick={() => setShowForm(!showForm)}
                  className="toggle-form-button btn align-items-end"
                >
                  {showForm ? "Закрити форму" : "Замовити довідку"}
                </a>
              </div>

              {showForm && <RequestForm onSuccess={fetchOrders} />}

              <div className="orders-section secondary-bg-white">
                <h2 className="orders-title">Мої замовлення</h2>
                {<RequestList orders={orders} loading={loading} />}
              </div>
            </div>
          </main>
        </div>
      </div>
    </>
  );
}
