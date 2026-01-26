// import axios from "axios";
import axiosClient from "./axiosClient";
import { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import "./HomePage.css";
import EditBookModal from "./EditBookModal";

const HomePage = () => {
  const [books, setBooks] = useState([]);
  const [newBook, setNewBook] = useState({ title: "", author: "", price: "" });
  const [sortBy, setSortBy] = useState("");
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(10);
  const [pageStatus, setPageStatus] = useState({
    hasNext: true,
    hasPrevious: true,
  });

  const [editingBook, setEditingBook] = useState(null);

  const navigate = useNavigate();

  const handleClickNext = () => {
    if (pageStatus.hasNext) {
      setPage((p) => p + 1);
    }
  };

  const handleClickPrevious = () => {
    if (pageStatus.hasPrevious) {
      setPage((p) => p - 1);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      const [sort, order] = sortBy ? sortBy.split("-") : ["", ""];

      try {
        const params = {
          page,
          limit,
          title: search || "",
          sortBy: sort,
          order,
        };
        const res = await axiosClient.get("/Books", { params });
        const { items, hasNext, hasPrevious } = res.data.data;
        setBooks(items);
        setPageStatus({ hasNext, hasPrevious });
      } catch (error) {
        console.error("Lỗi khi fetch dữ liệu:", error);
      }
    };

    const delay = search !== "" ? 500 : 0;
    const timeoutId = setTimeout(() => {
      fetchData();
    }, delay);

    return () => clearTimeout(timeoutId);
  }, [page, limit, search, sortBy]);

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
        await axiosClient.delete(`/Books/${id}`);
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
      await axiosClient.post("/Books", newBook);
      setPage(1);
      setLimit(10);
      setNewBook({ title: "", author: "", price: "" });
      alert("Thêm thành công");
    } catch (error) {
      alert("Thêm thất bại");
      console.error(error);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    navigate("/login");
  };

  const handleRegister = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    navigate("/register");
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

      case "price-asc": {
        setSortBy("price-asc");
        break;
      }

      case "price-desc": {
        setSortBy("price-desc");
        break;
      }

      default: {
        setSortBy("");
        break;
      }
    }
  };

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
    setPage(1);
  };

  const handleUpdate = (book) => {
    const allowUser = JSON.parse(localStorage.getItem("user"));

    if (!allowUser) {
      alert("Bạn phải đăng nhập để thực hiện thao tác này");
      navigate("/login");
      return;
    }

    if (allowUser.role !== "Admin") {
      alert("Chỉ Admin mới có quyền sửa sách!");
      return;
    }

    setEditingBook(book);
  };

  const getGreetingByLocalTime = () => {
    const h = new Date().getHours();

    if (h < 10) return "Chào buổi sáng";
    if (h < 13) return "Chào buổi trưa";
    if (h < 18) return "Chào buổi chiều";
    return "Chào buổi tối";
  };

  const getCurrentPosition = () =>
    new Promise((resolve, reject) => {
      if (!navigator.geolocation) {
        reject("Geolocation not supported");
      }

      navigator.geolocation.getCurrentPosition(
        (pos) => {
          resolve({
            lat: pos.coords.latitude,
            lon: pos.coords.longitude,
          });
        },
        (err) => reject(err),
        { enableHighAccuracy: true, timeout: 10000 },
      );
    });

  const API_KEY = import.meta.env.VITE_WEATHER_KEY;

  const fetchWeatherByLocation = async () => {
    const { lat, lon } = await getCurrentPosition();

    const res = await fetch(
      `https://api.openweathermap.org/data/2.5/weather?lat=${lat}&lon=${lon}&appid=${API_KEY}&units=metric&lang=vi`,
    );

    return res.json();
  };

  const fetchWeatherByDefault = async () => {
    const res = await fetch(
      `https://api.openweathermap.org/data/2.5/weather?q=Hanoi&appid=${API_KEY}&units=metric&lang=vi`,
    );
    return res.json();
  };

  const [weather, setWeather] = useState(null);

  useEffect(() => {
    fetchWeatherByLocation()
      .then(setWeather)
      .catch(() => {
        console.warn("Không lấy được vị trí, dùng mặc định");
        return fetchWeatherByDefault();
      })
      .then((data) => {
        if (data) setWeather(data);
      })
      .catch((err) => console.error("Lỗi fetch thời tiết:", err));
  }, []);

  const weatherIconMap = {
    Clear: "☀️",
    Clouds: "☁️",
    Rain: "🌧️",
    Thunderstorm: "⛈️",
    Snow: "❄️",
    Mist: "🌫️",
  };

  const getWeatherIcon = (main) => weatherIconMap[main] || "🌤️";

  const capitalize = (text) =>
    text ? text.charAt(0).toUpperCase() + text.slice(1) : "";

  const weatherMain = weather?.weather?.[0]?.main;
  const user = JSON.parse(localStorage.getItem("user"));

  const handleSubmitUpdate = async (updatedBook) => {
    try {
      await axiosClient.put(`/Books/${updatedBook.id}`, updatedBook);

      setBooks(books.map((b) => (b.id === updatedBook.id ? updatedBook : b)));

      alert("Cập nhật sách thành công");
      setEditingBook(null);
    } catch (err) {
      alert("Cập nhật thất bại");
      console.error(err);
    }
  };

  return (
    <div className="home-container">
      <div className="header">
        {user ? (
          <>
            <div>
              <h3>
                {getGreetingByLocalTime()}, {user?.name} 👋
              </h3>

              {weather && (
                <div className="weather-info">
                  <span>{weather.name},</span>
                  <span>
                    {capitalize(weather.weather[0].description)}{" "}
                    {getWeatherIcon(weatherMain)},
                  </span>
                  <span>{Math.round(weather.main.temp)}°C</span>
                </div>
              )}
            </div>
            <button className="logout-btn" onClick={handleLogout}>
              Đăng xuất
            </button>
          </>
        ) : (
          <>
            <h3>Xin chào 👋</h3>
            <div>
              <button className="logout-btn" onClick={handleLogout}>
                Đăng nhập
              </button>
              <button
                className="logout-btn"
                style={{ marginLeft: 10 }}
                onClick={handleRegister}
              >
                Đăng ký
              </button>
            </div>
          </>
        )}
      </div>

      {user?.role === "Admin" && (
        <div className="admin-section">
          <h2>Thêm sách mới</h2>
          <form onSubmit={addBookHandle}>
            <div className="form-group">
              <label>Title</label>
              <input
                name="title"
                placeholder="Nhập tên sách"
                value={newBook.title}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Author</label>
              <input
                name="author"
                placeholder="Nhập tên tác giả"
                value={newBook.author}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Price</label>
              <input
                type="number"
                name="price"
                placeholder="Nhập giá"
                value={newBook.price}
                onChange={handleInputChange}
                required
              />
            </div>
            <button type="submit" className="submit-btn">
              Thêm sách
            </button>
          </form>
        </div>
      )}

      <div className="main-section">
        <h1 style={{ fontWeight: "bold" }}>Thư viện sách</h1>

        <div className="filters">
          <div className="filter-group">
            <label>Tìm kiếm</label>
            <input
              type="text"
              placeholder="Nhập tên sách..."
              onChange={handleSearchChange}
              value={search}
            />
          </div>
          <div className="filter-group">
            <label>Sắp xếp</label>
            <select onChange={handleSortBy} value={sortBy}>
              <option value="">Chọn sắp xếp</option>
              <option value="author-asc">Tác giả: A → Z</option>
              <option value="author-desc">Tác giả: Z → A</option>
              <option value="title-asc">Tên sách: A → Z</option>
              <option value="title-desc">Tên sách: Z → A</option>
              <option value="price-asc">Giá sách: Thấp → Cao</option>
              <option value="price-desc">Giá sách: Cao → Thấp</option>
            </select>
          </div>
        </div>

        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Tên sách</th>
              <th>Tác giả</th>
              <th>Giá</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {books.length === 0 ? (
              <>
                <tr>
                  <td colSpan={5} style={{ textAlign: "center", padding: 16 }}>
                    Không có kết quả phù hợp
                  </td>
                </tr>

                {Array.from({ length: limit - 1 }).map((_, idx) => (
                  <tr key={`empty-${idx}`} className="empty-row">
                    <td colSpan={5}></td>
                  </tr>
                ))}
              </>
            ) : (
              <>
                {books.map((book) => (
                  <tr key={book.id}>
                    <td>{book.id}</td>
                    <td>{book.title}</td>
                    <td>{book.author}</td>
                    <td>{book.price.toLocaleString()} đ</td>
                    <td>
                      <div className="action-buttons">
                        <button
                          className="edit-btn"
                          onClick={() => handleUpdate(book)}
                        >
                          Sửa
                        </button>
                        <button
                          className="delete-btn"
                          onClick={() => deleteBookHandle(book.id)}
                        >
                          Xoá
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
                {Array.from({ length: 10 - books.length }).map((_, idx) => (
                  <tr key={`empty-${idx}`} className="empty-row">
                    <td colSpan="5"></td>
                  </tr>
                ))}
              </>
            )}
          </tbody>
        </table>

        <div className="pagination">
          <button
            onClick={handleClickPrevious}
            disabled={!pageStatus.hasPrevious}
          >
            Trang trước
          </button>
          <button onClick={handleClickNext} disabled={!pageStatus.hasNext}>
            Trang sau
          </button>
        </div>
      </div>

      {editingBook && (
        <EditBookModal
          book={editingBook}
          onClose={() => setEditingBook(null)}
          onSubmit={handleSubmitUpdate}
        />
      )}
    </div>
  );
};

export default HomePage;
