import { NavLink } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

interface SidebarProps {
  isOffcanvas?: boolean;
}

export default function Sidebar({ isOffcanvas = false }: SidebarProps) {
  const { logout } = useAuth();

  const handleLogout = (e: any) => {
    e.preventDefault();
    logout();
  };

  const sidebarContent = (
    <div>
      <div className="sidebar-title mb-4 px-3">
        <h5 className="fw-bold mb-0">Кабінет студента</h5>
      </div>

      <ul className="nav flex-column px-3">
        <li className="nav-item mb-2">
          <NavLink
            className={({ isActive }) =>
              `nav-link ${
                isActive ? "active fw-semibold text-primary" : "text-dark"
              }`
            }
            to="/dashboard"
          >
            Замовлення довідок
          </NavLink>
        </li>
        <li className="nav-item mb-2">
          <NavLink
            className={({ isActive }) =>
              `nav-link ${
                isActive ? "active fw-semibold text-primary" : "text-dark"
              }`
            }
            to="/account"
          >
            Налаштування акаунта
          </NavLink>
        </li>
        <li className="nav-item mt-3">
          <a
            className="nav-link text-danger fw-semibold"
            href="#"
            onClick={handleLogout}
          >
            Вихід
          </a>
        </li>
      </ul>
    </div>
  );

  if (isOffcanvas) {
    return (
      <div
        className="offcanvas offcanvas-start"
        tabIndex={-1}
        id="mobileSidebar"
        aria-labelledby="mobileSidebarLabel"
      >
        <div className="offcanvas-header">
          <h5 className="offcanvas-title" id="mobileSidebarLabel">
            Меню
          </h5>
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
