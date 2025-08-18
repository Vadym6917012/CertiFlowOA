const RequestList = () => {
  return (
    <>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Тип довідки</th>
            <th scope="col">Формат</th>
            <th scope="col">Дата</th>
            <th scope="col">Статус</th>
            <th scope="col">Дії</th>
          </tr>
        </thead>
        <tbody>
            <tr>
                <th>Довідка про навчання</th>
                <th>Довідка Паперовий</th>
                <th>2024-03-10</th>
                <th>В обробці</th>
                <th><a href="">Download</a></th>
            </tr>
        </tbody>
      </table>
    </>
  );
};

export default RequestList;
