import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginPage from "../Pages/LoginPage";
import Layout from "../components/layout/Layout";
import Dashboard from "../Pages/StudentDashboard";
import OAuthRedirectPage from "../Pages/OAuthRedirectPage";
import ErrorPage from "../Pages/ErrorPage";
import ProtectedRoute from "./ProtectedRoute";

export default function AppRouter() {
  return (
    <BrowserRouter>
  <Routes>
    <Route path="/sign-in" element={<LoginPage />} />
    <Route path="/redirect" element={<OAuthRedirectPage />} />
    
    <Route
      path="/"
      element={
        <ProtectedRoute allowedRoles={["Admin", "Student"]}>
          <Layout />
        </ProtectedRoute>
      }
    >
      {/* дефолтний роут */}
      <Route
        index
        element={
          <ProtectedRoute allowedRoles={["Admin", "Student"]}>
            <Dashboard />
          </ProtectedRoute>
        }
      />

      {/* доступ за /dashboard */}
      <Route
        path="dashboard"
        element={
          <ProtectedRoute allowedRoles={["Admin", "Student"]}>
            <Dashboard />
          </ProtectedRoute>
        }
      />
    </Route>

    <Route path="/403" element={<ErrorPage />} />
    <Route path="*" element={<ErrorPage />} />
  </Routes>
</BrowserRouter>
  );
}
