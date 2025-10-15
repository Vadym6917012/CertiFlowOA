import { Order } from "../api/types/order";
import apiClient from "../api/apiClient";
import { toast } from "react-toastify";

interface RequestListProps {
  orders: Order[];
  loading: boolean;
}

const RequestList = ({ orders, loading }: RequestListProps) => {
  const handleDownload = async (documentId: number, documentType: string) => {
    try {
      const response = await apiClient.get(
        `/documents/${documentId}/download`,
        {
          responseType: "blob",
        }
      );

      // Створюємо URL для завантаження
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `${documentType}_${documentId}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);

      toast.success("Документ завантажено успішно");
    } catch (err) {
      toast.error("Не вдалося завантажити документ");
      console.error("Error downloading document:", err);
    }
  };

  const getStatusBadgeClass = (status: string | null) => {
    if (!status) return "badge bg-secondary";

    switch (status.toLowerCase()) {
      case "новий":
        return "badge bg-primary";
      case "в обробці":
        return "badge bg-warning text-dark";
      case "підписано":
        return "badge bg-success";
      case "відхилено":
        return "badge bg-danger";
      case "завершено":
        return "badge bg-success";
      default:
        return "badge bg-secondary";
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("uk-UA");
  };

  const canDownloadDocument = (status: string | null) => {
    if (!status) return false;
    const downloadableStatuses = ["підписано", "завершено"];
    return downloadableStatuses.includes(status.toLowerCase());
  };

  if (loading) {
    return (
      <div className="d-flex justify-content-center">
        <div className="spinner-border" role="status">
          <span className="visually-hidden">Завантаження...</span>
        </div>
      </div>
    );
  }

  if (orders.length === 0) {
    return (
      <div className="alert alert-info" role="alert">
        <i className="bi bi-info-circle me-2"></i>У вас ще немає замовлень
      </div>
    );
  }

  return (
    <div className="card border-0">
      <div className="card-body p-0">
        <div className="table-responsive">
          <table className="student-orders-table table table-striped table-hover mb-0">
            <thead>
              <tr>
                <th scope="col">Тип довідки</th>
                <th scope="col">Формат</th>
                <th scope="col">Мета отримання</th>
                <th scope="col">Дата створення</th>
                <th scope="col">Статус</th>
                <th scope="col" className="text-end">
                  Дії
                </th>
              </tr>
            </thead>
            <tbody>
              {orders.map((order) => (
                <tr key={order.orderId}>
                  <td>
                    {order.documentTypeName}
                  </td>
                  <td>
                    <span
                      className={`badge ${
                        order.format?.toLowerCase().includes("цифров")
                          ? "bg-info text-dark"
                          : "bg-secondary"
                      }`}
                    >
                      {order.format?.toLowerCase().includes("цифров")
                        ? "Цифровий"
                        : "Паперовий"}
                    </span>
                  </td>
                  <td>{order.purpose}</td>
                  <td>
                    <span className="text-nowrap">
                      {formatDate(order.createdDate)}
                    </span>
                  </td>
                  <td>
                    <span className={getStatusBadgeClass(order.documentStatus)}>
                      {order.documentStatus}
                    </span>
                  </td>
                  <td>
                    {canDownloadDocument(order.documentStatus) ? (
                      <button
                        className="btn btn-sm btn-outline-primary"
                        onClick={() =>
                          handleDownload(
                            order.documentId,
                            order.documentTypeName
                          )
                        }
                        title="Завантажити документ"
                      >
                        <i className="bi bi-download me-1"></i>
                        Завантажити
                      </button>
                    ) : (
                      <span className="text-muted small">
                        <i className="bi bi-clock me-1"></i>
                        Очікує обробки
                      </span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
      <div className="card-footer text-muted">
        <small>
          Показано {orders.length}{" "}
          {orders.length === 1 ? " замовлення" : "замовлень"}
        </small>
      </div>
    </div>
  );
};

export default RequestList;
