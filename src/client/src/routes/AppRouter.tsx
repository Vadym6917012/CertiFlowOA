import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "../Pages/LoginPage";
import Layout from "../components/layout/Layout";
import OAuthRedirectPage from "../Pages/OAuthRedirectPage";
import ErrorPage from "../Pages/ErrorPage";
import ProtectedRoute from "./ProtectedRoute";
import AccountPage from "../Pages/AccountPage";
import StudentDashboard from "../Pages/StudentDashboard";
import AdminLayout from "../components/layout/AdminLayout";
import AdminDashboard from "../Pages/Admin/AdminDashboard";

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
          <Route
            index
            element={
              <ProtectedRoute allowedRoles={["Admin", "Student"]}>
                <StudentDashboard />
              </ProtectedRoute>
            }
          />

          <Route
            path="dashboard"
            element={
              <ProtectedRoute allowedRoles={["Admin", "Student"]}>
                <StudentDashboard />
              </ProtectedRoute>
            }
          />

          <Route
            path="account"
            element={
              <ProtectedRoute allowedRoles={["Admin", "Student"]}>
                <AccountPage />
              </ProtectedRoute>
            }
          />
          <Route path="home" element={<Navigate to="/" replace />} />
        </Route>

        <Route
          path="/admin"
          element={
            <ProtectedRoute allowedRoles={["Admin"]}>
              <AdminLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<AdminDashboard />} />
          <Route path="dashboard" element={<AdminDashboard />} />
          {/* можна додати: */}
          {/* <Route path="users" element={<UsersPage />} /> */}
          {/* <Route path="orders" element={<OrdersPage />} /> */}
        </Route>

        <Route path="/403" element={<ErrorPage />} />
        <Route path="*" element={<ErrorPage />} />
      </Routes>
    </BrowserRouter>
  );
}
