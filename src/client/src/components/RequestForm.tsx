import { useCallback, useEffect, useState } from "react";
import { toast } from "react-toastify";
import apiClient from "../api/apiClient";
import { DocumentFormat } from "../api/types/documentFormat";
import { User } from "../api/types/user";
import { DocumentType } from "../api/types/documentType";

const API_ENDPOINTS = {
  DOCUMENT_TYPES: "/DocumentType",
  ORDERS: "/Orders/create",
};

interface RequestFormProps {
  onSuccess: () => void;
}

const RequestForm = ({ onSuccess }: RequestFormProps) => {
  const [format, setFormat] = useState<DocumentFormat>();
  const [purpose, setPurpose] = useState("");
  const [documentTypes, setDocumentTypes] = useState<DocumentType[]>([]);
  const [selectedType, setSelectedType] = useState<number | "">(""); // Changed to handle empty string
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const fetchDocumentTypes = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await apiClient.get<DocumentType[]>(API_ENDPOINTS.DOCUMENT_TYPES);
      setDocumentTypes(response.data);
      console.log("Document types loaded:", response.data);
    } catch (error) {
      console.error("Error loading document types:", error);
      toast.error("Не вдалося завантажити типи документів");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDocumentTypes();
  }, [fetchDocumentTypes]);

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();

      if (selectedType === "" || !format || !purpose.trim()) {
        console.log(selectedType, format, purpose);
        toast.error("Будь ласка, заповніть всі поля!", { position: "bottom-right" });
        return;
      }

      setIsSubmitting(true);

      try {
        const user: User | null = JSON.parse(localStorage.getItem("user") || "null");
        const token = user?.accessToken;

        if (!token) {
          toast.error("Користувача не авторизований.", { position: "bottom-right" });
          return;
        }

        await apiClient.post(API_ENDPOINTS.ORDERS, {
          documentTypeId: selectedType,
          purpose: purpose.trim(),
          format: format,
        });

        toast.success("Замовлення успішно створено!", { position: "bottom-right" });

        onSuccess?.();

        // Reset form
        setSelectedType("");
        setFormat(undefined);
        setPurpose("");
      } catch (error: any) {
        if (error.response?.data) {
          toast.error(error.response.data);
        } else {
          toast.error("Помилка при створенні замовлення. Будь ласка, спробуйте ще раз.");
        }
      } finally {
        setIsSubmitting(false);
      }
    },
    [selectedType, format, purpose, onSuccess]);

  if (isLoading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: '200px' }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="request-form-section secondary-bg-white">
      <div className="card-body">
        <h2 className="mb-4">Замовити довідку</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="documentType" className="form-label">Тип документа</label>
            <select
              id="documentType"
              className="form-select"
              value={selectedType}
              onChange={(e) => setSelectedType(e.target.value ? Number(e.target.value) : "")}
              required
              disabled={isSubmitting}
              aria-label="Оберіть тип документа"
            >
              <option value="">Оберіть тип документа</option>
              {documentTypes.map((docType) => (
                <option key={docType.documentTypeId} value={docType.documentTypeId}>
                  {docType.name}
                </option>
              ))}
            </select>
          </div>

          <div className="mb-3">
            <label className="form-label">Формат</label>
            <div className="d-flex gap-3">
              <div className="form-check">
                <input
                  type="radio"
                  id="paper"
                  name="format"
                  className="form-check-input"
                  checked={format === DocumentFormat.Paper}
                  onChange={() => setFormat(DocumentFormat.Paper)}
                  disabled={isSubmitting}
                  required
                />
                <label className="form-check-label" htmlFor="paper">
                  Паперовий
                </label>
              </div>
              <div className="form-check">
                <input
                  type="radio"
                  id="electronic"
                  name="format"
                  className="form-check-input"
                  checked={format === DocumentFormat.Electronic}
                  onChange={() => setFormat(DocumentFormat.Electronic)}
                  disabled={isSubmitting}
                />
                <label className="form-check-label" htmlFor="digital">
                  Цифровий
                </label>
              </div>
            </div>
          </div>

          <div className="mb-4">
            <label htmlFor="purpose" className="form-label">Призначення довідки</label>
            <input
              id="purpose"
              type="text"
              className="form-control"
              value={purpose}
              onChange={(e) => setPurpose(e.target.value)}
              placeholder="Наприклад: для подання до деканату"
              required
              disabled={isSubmitting}
              aria-describedby="purposeHelp"
            />
            <div id="purposeHelp" className="form-text">
              Будь ласка, опишіть, для чого вам потрібен цей документ
            </div>
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100 py-2"
            disabled={isSubmitting || isLoading}
          >
            {isSubmitting ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                Обробка...
              </>
            ) : (
              "Подати замовлення"
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default RequestForm;