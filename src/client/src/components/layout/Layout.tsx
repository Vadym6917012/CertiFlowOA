import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Sidebar from "../Sidebar";

export default function Layout() {
  return (
      <div className="container-fluid">
        <div className="row">
          <Sidebar />
          <Sidebar isOffcanvas />
          <main className="col-md-9 col-lg-10 ms-sm-auto px-md-4 py-4 min-vh-100 bg-light">
            <ToastContainer />
            <Outlet />
          </main>
        </div>
      </div>
  );
}
