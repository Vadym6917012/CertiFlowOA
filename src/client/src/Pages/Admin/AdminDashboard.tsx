export default function AdminDashboard() {
  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1 className="h4 fw-bold">Головна сторінка адміністратора</h1>
        <button
          className="btn btn-outline-primary d-md-none"
          type="button"
          data-bs-toggle="offcanvas"
          data-bs-target="#mobileAdminSidebar"
        >
          ☰ Меню
        </button>
      </div>

      <p className="text-muted">
        Тут ви можете переглядати заявки студентів, керувати користувачами та
        налаштовувати систему.
      </p>
    </div>
  );
}