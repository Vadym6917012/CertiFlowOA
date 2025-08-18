import { useCallback, useEffect, useState } from "react";
import { toast } from "react-toastify";
import apiClient from "../api/apiClient";

type DocumentType = {
  documentTypeId: number;
  name: string;
};

type DocumentFormat = "paper" | "digital";

interface User {
  accessToken: string;
}

const API_ENDPOINTS = {
  DOCUMENT_TYPES: "/DocumentType",
  ORDERS: "/Orders/create",
};

const RequestForm = () => {
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
      toast.error("Failed to load document types");
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

      if (selectedType === "" || !format || !purpose) {
        toast.error("Please fill all fields!", { position: "bottom-right" });
        return;
      }

      setIsSubmitting(true);

      try {
        const user: User | null = JSON.parse(localStorage.getItem("user") || "null");
        const token = user?.accessToken;

        if (!token) {
          toast.error("User not authenticated.", { position: "bottom-right" });
          return;
        }

        await apiClient.post(API_ENDPOINTS.ORDERS, {
          documentTypeId: selectedType,
          purpose: purpose.trim(),
          format: format,
        });

        toast.success("Order created successfully!", { position: "bottom-right" });

        // Reset form
        setSelectedType("");
        setFormat(undefined);
        setPurpose("");
      } catch (error) {
        console.error("Order creation error:", error);
        toast.error("Error creating order. Please try again.");
      } finally {
        setIsSubmitting(false);
      }
    },
    [selectedType, format, purpose]
  );

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
        <h2 className="mb-4">Request Document</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="documentType" className="form-label">Document Type</label>
            <select
              id="documentType"
              className="form-select"
              value={selectedType}
              onChange={(e) => setSelectedType(e.target.value ? Number(e.target.value) : "")}
              required
              disabled={isSubmitting}
              aria-label="Select document type"
            >
              <option value="">Select document type</option>
              {documentTypes.map((docType) => (
                <option key={docType.documentTypeId} value={docType.documentTypeId}>
                  {docType.name}
                </option>
              ))}
            </select>
          </div>

          <div className="mb-3">
            <label className="form-label">Format</label>
            <div className="d-flex gap-3">
              <div className="form-check">
                <input
                  type="radio"
                  id="paper"
                  name="format"
                  value="paper"
                  className="form-check-input"
                  checked={format === "paper"}
                  onChange={() => setFormat("paper")}
                  disabled={isSubmitting}
                  required
                />
                <label className="form-check-label" htmlFor="paper">
                  Paper
                </label>
              </div>
              <div className="form-check">
                <input
                  type="radio"
                  id="digital"
                  name="format"
                  value="digital"
                  className="form-check-input"
                  checked={format === "digital"}
                  onChange={() => setFormat("digital")}
                  disabled={isSubmitting}
                />
                <label className="form-check-label" htmlFor="digital">
                  Digital
                </label>
              </div>
            </div>
          </div>

          <div className="mb-4">
            <label htmlFor="purpose" className="form-label">Purpose</label>
            <input
              id="purpose"
              type="text"
              className="form-control"
              value={purpose}
              onChange={(e) => setPurpose(e.target.value)}
              placeholder="Example: for submission to the dean's office"
              required
              disabled={isSubmitting}
              aria-describedby="purposeHelp"
            />
            <div id="purposeHelp" className="form-text">
              Please describe what you need this document for
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
                Processing...
              </>
            ) : (
              "Submit Order"
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default RequestForm;