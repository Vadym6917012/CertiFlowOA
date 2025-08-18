import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export const useAuth = () => {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem("user");
    toast.info("Ви вийшли з акаунту", {
      position: "bottom-right",
    });
    navigate("/sign-in", { replace: true });
  };

  return { logout };
};