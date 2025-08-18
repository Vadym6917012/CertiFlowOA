import { FC, ReactNode, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { User } from "../Pages/OAuthRedirectPage";
import { jwtDecode } from "jwt-decode";

interface ProtectedRouteProps {
  children: ReactNode;
  allowedRoles?: string[];
}

const ProtectedRoute: FC<ProtectedRouteProps> = ({
  children,
  allowedRoles,
}) => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const userJson = localStorage.getItem("user");
    const user = userJson ? (JSON.parse(userJson) as User) : null;

    if (!user) {
      navigate("/sign-in", { replace: true });
      return;
    }

    const token = getDecodedAccessToken(user?.accessToken as string);
    const userRole = token?.role;

    if (!userRole || (allowedRoles && !allowedRoles.includes(userRole))) {
      navigate("/403");
      return;
    }

    setIsLoading(false);
  }, [allowedRoles, navigate]);

  if (isLoading) {
    return <p>Loading...</p>;
  }

  return <>{children}</>;
};

function getDecodedAccessToken(token: string): any {
  try {
    return jwtDecode(token);
  } catch (error) {
    console.error("Invalid token", error);
    return null;
  }
}

export default ProtectedRoute;
