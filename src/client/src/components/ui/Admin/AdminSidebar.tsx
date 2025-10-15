import { NavLink } from "react-router-dom";
import { useAuth } from "../../../hooks/useAuth";

interface AdminSidebarProps {
  isOffcanvas?: boolean;
}

export default function AdminSidebar({ isOffcanvas = false }: AdminSidebarProps) {
  const { logout } = useAuth();

  const handleLogout = (e: React.MouseEvent) => {
    e.preventDefault();
    logout();
  };

  const sidebarContent = (
    <div>
      <div className="px-3 mb-4">
        <h5 className="fw-bold mb-0">Панель адміністратора</h5>
      </div>

      <ul className="nav flex-column px-3">
        <li className="nav-item mb-2">
          <NavLink
            to="/admin/dashboard"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active text-primary fw-semibold" : "text-dark"}`
            }
          >
            Головна
          </NavLink>
        </li>
        <li className="nav-item mb-2">
          <NavLink
            to="/admin/orders"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active text-primary fw-semibold" : "text-dark"}`
            }
          >
            Замовлення
          </NavLink>
        </li>
        <li className="nav-item mb-2">
          <NavLink
            to="/admin/users"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active text-primary fw-semibold" : "text-dark"}`
            }
          >
            Користувачі
          </NavLink>
        </li>
        <li className="nav-item mt-3">
          <a href="#" className="nav-link text-danger fw-semibold" onClick={handleLogout}>
            Вихід
          </a>
        </li>
      </ul>
    </div>
  );

  if (isOffcanvas) {
    return (
      <div className="offcanvas offcanvas-start" tabIndex={-1} id="mobileAdminSidebar">
        <div className="offcanvas-header">
          <h5 className="offcanvas-title">Меню адміністратора</h5>
          <button
            type="button"
            className="btn-close"
            data-bs-dismiss="offcanvas"
            aria-label="Закрити"
          ></button>
        </div>
        <div className="offcanvas-body">{sidebarContent}</div>
      </div>
    );
  }

  return (
    <nav className="col-md-3 col-lg-2 d-none d-md-block bg-light border-end min-vh-100 py-4">
      {sidebarContent}
    </nav>
  );
}