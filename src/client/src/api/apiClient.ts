import axios, { AxiosRequestConfig, AxiosResponse, AxiosError, InternalAxiosRequestConfig } from "axios";
import { toast } from "react-toastify";

const BASE_URL = import.meta.env.VITE_API_BASE_URL || "https://localhost:7195/api";

const apiClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 10000,
});

// Request interceptor with correct typing
apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    let token: string | null = null;
    try {
      const user = JSON.parse(localStorage.getItem("user") || "{}");
      token = user?.accessToken || null;
    } catch (error) {
      console.error("Error parsing user data", error);
    }

    if (token) {
      config.headers = config.headers || {};
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error: AxiosError) => {
    if (error.response) {
      const status = error.response.status;
      const data = error.response.data as { message?: string; errors?: Record<string, string[]> };

      switch (status) {
        case 400:
          if (data.errors) {
            const errorMessages = Object.values(data.errors).flat();
            toast.error(errorMessages.join("\n"));
          } else {
            toast.error(data.message || "Bad request");
          }
          break;
        case 401:
          toast.error("Unauthorized access. Please login again.");
          break;
        case 403:
          toast.error("Forbidden");
          break;
        case 404:
          toast.error("Resource not found");
          break;
        case 500:
          toast.error(data.message || "Server error");
          break;
        default:
          toast.error(data.message || `API error: ${status}`);
      }
    } else if (error.request) {
      toast.error("Network error. Please check your connection.");
    } else {
      toast.error("Request setup error");
    }

    return Promise.reject(error);
  }
);

// Typed wrapper functions
export const apiGet = async <T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return apiClient.get<T>(url, config);
};

export const apiPost = async <T>(
  url: string,
  data?: any,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return apiClient.post<T>(url, data, config);
};

export const apiPut = async <T>(
  url: string,
  data?: any,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return apiClient.put<T>(url, data, config);
};

export const apiDelete = async <T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return apiClient.delete<T>(url, config);
};

export default apiClient;