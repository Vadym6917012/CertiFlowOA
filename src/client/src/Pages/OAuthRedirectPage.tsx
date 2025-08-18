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
      console.log("useEffect triggered");

      const urlParams = new URLSearchParams(window.location.search);
      const authorizationCode = urlParams.get("code");

      if (authorizationCode) {
        const codeVerifier = localStorage.getItem("codeVerifier");
        if (!codeVerifier){
          setError("Code verifier missing.");
          setLoading(false);
          return;
        }
        exchangeCodeForToken(authorizationCode, codeVerifier);
      } else {
        setError("Authorization code missing.");
        setLoading(false);
      }
  }, []);

  const exchangeCodeForToken = useCallback(
     async ( authorizationCode: string, codeVerifier: string | null) => {
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
      navigate("/dashboard");
      toast.success('Ласкаво просимо!', {
        position: 'bottom-right',
      });
      <ToastContainer />
    } catch (error) {
      console.error("Error exchanging code for token", error);
      setError("There was an error processing your request.");
    } finally {
      setLoading(false);
    }
  },
  [navigate]
);

  return (
    <>
      {loading ? (
        <>
          <h1>Just a moment...</h1>
          <div className="slider">
            <div className="line"></div>
            <div className="break dot1"></div>
            <div className="break dot2"></div>
            <div className="break dot3"></div>
          </div>
          <p>
            We're redirecting you to our new site... Not working?{" "}
            <a href="#">Click here.</a>
          </p>
        </>
      ) : error ? (
        <div className="error-message">
          <h2>Error</h2>
          <p>{error}</p>
        </div>
      ) : (
        <div>
          <h1>Successfully Logged In</h1>
          <p>Redirecting...</p>
        </div>
      )}
    </>
  );
}
