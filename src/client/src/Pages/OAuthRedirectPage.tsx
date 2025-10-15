import axios from "axios";
import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";

export interface User {
  accessToken: string;
  name: string;
  email: string;
  role: string;
  pictureUrl: string;
}

export default function OAuthRedirectPage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const authorizationCode = urlParams.get("code");

    if (!authorizationCode) {
      setError("Authorization code missing.");
      setLoading(false);
      return;
    }

    const codeVerifier = localStorage.getItem("codeVerifier");
    if (!codeVerifier) {
      setError("Code verifier missing.");
      setLoading(false);
      return;
    }

    exchangeCodeForToken(authorizationCode, codeVerifier);
  }, []);

  const exchangeCodeForToken = useCallback(
    async (authorizationCode: string, codeVerifier: string) => {
      try {
        const response = await axios.post(
          "https://localhost:7195/api/GoogleOAuth/login",
          { authorizationCode, codeVerifier },
          { headers: { "Content-Type": "application/json" } }
        );

        const data = await response.data;

        const user: User = {
          accessToken: data.accessToken,
          name: data.name,
          email: data.email,
          pictureUrl: data.pictureUrl,
          role: data.role,
        };

        localStorage.setItem("user", JSON.stringify(user));
        localStorage.removeItem("codeVerifier");

        if (user.role === "Admin") {
          navigate("/admin", { replace: true });
        } else {
          navigate("/dashboard", { replace: true });
        }

        toast.success(`Ласкаво просимо, ${user.name}!`, {
          position: "bottom-right",
        });
        <ToastContainer />;
      } catch (error) {
        console.error("Error exchanging code for token", error);
        setError("There was an error processing your request.");
      } finally {
        setLoading(false);
      }
    },
    [navigate]
  );

  if (loading) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-100">
        <h1 className="mb-4">Just a moment...</h1>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-3 text-muted">
          Redirecting you to the application. If it takes too long, try refreshing the page.
        </p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-100 text-center">
        <h2 className="text-danger mb-3">Error</h2>
        <p>{error}</p>
      </div>
    );
  }

  return null;
}
