import { useAuth } from "../hooks/useAuth";

export default function Sidebar({ isOffcanvas = false }) {
    const { logout } = useAuth();

    const handleLogout = (e:any) => {
        e.preventDefault();
        logout();
    }

    const sidebarTitle = (
    <div className="sidebar-title mb-4 px-3">
      <h5 className="fw-bold mb-0">Кабінет студента</h5>
    </div>
  );

  const sidebarContent = (
    <>
      {sidebarTitle}
      <ul className="nav flex-column px-3">
        <li className="nav-item">
          <a className="nav-link active" href="#">
            Item 1
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link" href="#">
            Item 2
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link" href="#">
            Item 3
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link text-danger" href="#" onClick={handleLogout}>
            Вихід
          </a>
        </li>
      </ul>
    </>
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
    <nav className="col-md-3 col-lg-2 d-none d-md-block bg-light sidebar py-4">
      {sidebarContent}
    </nav>
  );
}
