import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import AdminSidebar from "../ui/Admin/AdminSidebar";

export default function AdminLayout() {
  return (
    <div className="container-fluid">
      <div className="row">
        <AdminSidebar />
        <AdminSidebar isOffcanvas />

        <main className="col-md-9 col-lg-10 ms-sm-auto px-md-4 py-4 min-vh-100 bg-light">
          <ToastContainer />
          <Outlet />
        </main>
      </div>
    </div>
  );
}