import axios from "axios";
import { useEffect, useState, useCallback } from "react";
import { Navigate, useNavigate } from "react-router-dom";

const HomePage = () => {
  const [books, setBooks] = useState([]);
  const [newBook, setNewBook] = useState({ title: "", author: "", price: "" });
  const [sortBy, setSortBy] = useState("");
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const fetchData = useCallback(async () => {
    const [sort, order] = sortBy ? sortBy.split("-") : ["", ""];

    try {
      const res = await axios.get(
        `https://localhost:7216/api/v1/Books?page=1&limit=10&title=&sortBy=${sort}&order=${order}`
      );
      setBooks(res.data.data);
    } catch (error) {
      console.error("Lỗi khi fetch dữ liệu:", error);
    }
  }, [sortBy]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const deleteBookHandle = async (id) => {
    const allowUser = JSON.parse(localStorage.getItem("user"));

    if (!allowUser) {
      alert("Bạn phải đăng nhập để thực hiện thao tác này");
      navigate("/login");
      return;
    }

    if (allowUser.role !== "Admin") {
      alert("Chỉ Admin mới có quyền xóa sách!");
      return;
    }

    if (window.confirm("Bạn có chắc muốn xoá?")) {
      try {
        await axios.delete(`https://localhost:7216/api/v1/Books/${id}`);
        setBooks(books.filter((b) => b.id !== id));
        alert("Xoá thành công!");
      } catch (error) {
        alert("Xoá thất bại");
        console.error(error);
      }
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewBook({ ...newBook, [name]: value });
  };

  const addBookHandle = async (e) => {
    e.preventDefault();
    try {
      await axios.post("https://localhost:7216/api/v1/Books/", newBook);
      fetchData();
      setNewBook({ title: "", author: "", price: "" });
      alert("Thêm thành công");
    } catch (error) {
      alert("Thêm thất bại");
      console.error(error);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("user");
    navigate("/login");
  };

  const handleSortBy = (e) => {
    switch (e.target.value) {
      case "author-asc": {
        setSortBy("author-asc");
        break;
      }

      case "author-desc": {
        setSortBy("author-desc");
        break;
      }
      case "title-asc": {
        setSortBy("title-asc");
        break;
      }

      case "title-desc": {
        setSortBy("title-desc");
        break;
      }

      default: {
        setSortBy("");
        break;
      }
    }
  };

  const handleSearch = useCallback(async () => {
    const [sort, order] = sortBy ? sortBy.split("-") : ["", ""];
    try {
      const res = await axios.get(
        `https://localhost:7216/api/v1/Books?page=1&limit=10&title=${search}&sortBy=${sort}&order=${order}`
      );
      setBooks(res.data.data);
    } catch (error) {
      console.error("Lỗi khi fetch:", error);
    }
  }, [sortBy, search]);

  // const handleOrder = () => {
  //   setOrder((o) => (b.order = order));
  // };

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      handleSearch();
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [search, sortBy, handleSearch]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  };

  const user = JSON.parse(localStorage.getItem("user"));

  return (
    <>
      <button onClick={handleLogout} style={{ float: "right" }}>
        Đăng xuất
      </button>
      {user?.role === "Admin" && (
        <div>
          <h1>Thêm sách mới</h1>
          <label>Title: </label>
          <form onSubmit={addBookHandle}>
            <input
              name="title"
              placeholder="Title"
              value={newBook.title}
              onChange={handleInputChange}
              required
            />
            <br></br>
            <label>Author: </label>
            <input
              name="author"
              placeholder="Author"
              value={newBook.author}
              onChange={handleInputChange}
              required
            />
            <br></br>
            <label>Price: </label>
            <input
              type="number"
              name="price"
              placeholder="Price"
              value={newBook.price}
              onChange={handleInputChange}
              required
            />
            <br></br>
            <button type="submit">Thêm sách</button>
          </form>
        </div>
      )}
      <h1>Danh sách sách thư viện</h1>
      <span>Tìm kiếm </span>
      <input
        type="text"
        name="find"
        onChange={handleSearchChange}
        value={search}
      ></input>
      {/* <button onClick={handleSearch} value={search}>
        Tìm kiếm
      </button> */}
      <br></br>
      <span>Sắp xếp theo </span>
      <select onChange={handleSortBy} value={sortBy}>
        <option value="">Chọn sắp xếp theo</option>
        <option value="author-asc">Tên tác giả tăng dần</option>
        <option value="author-desc">Tên tác giả giảm dần</option>
        <option value="title-asc">Tên sách tăng dần</option>
        <option value="title-desc">Tên sách giảm dần</option>
      </select>

      <hr></hr>

      <table border="1" style={{ width: "100%" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Title</th>
            <th>Author</th>
            <th>Price</th>
            <th>Method</th>
          </tr>
        </thead>
        <tbody>
          {books.map((book) => (
            <tr key={book.id}>
              <td>{book.id}</td>
              <td>{book.title}</td>
              <td>{book.author}</td>
              <td>{book.price.toLocaleString()} đ</td>
              <td style={{ textAlign: "center" }}>
                <button style={{ marginRight: "10px" }}>Sửa</button>
                <button onClick={() => deleteBookHandle(book.id)}>Xoá</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default HomePage;
