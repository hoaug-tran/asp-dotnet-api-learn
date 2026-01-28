import axiosClient from "../../api/axiosClient";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../style/HomePage.css";
import EditBookModal from "../common/EditBookModal";
import { NotificationContext } from "../common/NotificationContext";

const API_KEY = import.meta.env.VITE_WEATHER_KEY;

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

const UserPage = () => {
  const [users, setUsers] = useState([]);
  const [newUser, setNewUser] = useState({
    name: "",
    username: "",
    password: "",
    verifyPassword: "",
    email: "",
    phone: "",
    role: "",
    avatar: null,
  });
  const [sortBy, setSortBy] = useState("");
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(10);
  const [pageStatus, setPageStatus] = useState({
    hasNext: true,
    hasPrevious: true,
  });
  const [editingUser, setEditingUser] = useState(null);
  const [avatarFileName, setAvatarFileName] = useState("Chọn tập tin");
  const [isUploading, setIsUploading] = useState(false);
  const [avatarPreview, setAvatarPreview] = useState(null);

  const { notify } = useContext(NotificationContext);

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
          search,
          sortBy: sort,
          order,
        };
        const res = await axiosClient.get("/Users", { params });
        const { items, hasNext, hasPrevious } = res.data.data;
        setUsers(items);
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

  const handleInputChange = (e) => {
    const { name, value, type, files } = e.target;
    const fieldValue = type === "file" ? files?.[0] : value;
    setNewUser({ ...newUser, [name]: fieldValue });

    if (type === "file" && files?.[0]) {
      const file = files[0];
      if (!file) {
        return;
      }
      setNewUser((prev) => ({
        ...prev,
        avatar: file,
      }));
      setAvatarFileName(file.name);
      const reader = new FileReader();
      reader.onload = () => {
        setAvatarPreview(reader.result);
      };
      reader.onerror = () => {};
      reader.readAsDataURL(file);
    }
  };

  const resetForm = () => {
    setNewUser({
      name: "",
      username: "",
      password: "",
      verifyPassword: "",
      email: "",
      phone: "",
      role: "",
      avatar: null,
    });
    setAvatarFileName("Chọn tập tin");
    setAvatarPreview(null);
  };

  const addUserHandle = async (e) => {
    e.preventDefault();

    if (
      !newUser.name ||
      !newUser.username ||
      !newUser.password ||
      !newUser.role
    ) {
      notify("warning", "Vui lòng điền tất cả các trường bắt buộc!");
      return;
    }

    if (newUser.password !== newUser.verifyPassword) {
      notify("warning", "Mật khẩu không khớp!");
      return;
    }

    if (newUser.password.length < 6) {
      notify("warning", "Mật khẩu phải có ít nhất 6 ký tự!");
      return;
    }

    if (newUser.email && !/^[\s@]+@[\s@]+\.[\s@]+$/.test(newUser.email)) {
      notify("warning", "Email không hợp lệ!");
      return;
    }

    if (
      // nice regrex...
      newUser.phone &&
      !/^\d{10,11}$/.test(newUser.phone.replace(/\D/g, ""))
    ) {
      notify("warning", "Số điện thoại không hợp lệ!");
      return;
    }

    if (newUser.avatar && newUser.avatar.size > 5 * 1024 * 1024) {
      notify("warning", "Kích thước ảnh không được vượt quá 5MB!");
      return;
    }

    try {
      setIsUploading(true);
      const formData = new FormData();

      formData.append("name", newUser.name);
      formData.append("username", newUser.username);
      formData.append("password", newUser.password);
      formData.append("verifyPassword", newUser.verifyPassword);
      formData.append("email", newUser.email || "");
      formData.append("phone", newUser.phone || "");
      formData.append("role", newUser.role);

      if (newUser.avatar) {
        formData.append("avatar", newUser.avatar);
      }

      const res = await axiosClient.post("/Users", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      if (res.data.success) {
        notify("success", "Thêm thành viên mới thành công");
        resetForm();
        setPage(1);
        setLimit(10);
      } else {
        notify("error", res.data.message || "Thêm thành viên mới thất bại!");
      }
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Thêm thành viên mới thất bại!";
      notify("error", "Lỗi: ", errorMessage);
    } finally {
      setIsUploading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    navigate("/login");
  };

  const handleBack = () => {
    navigate("/");
  };

  const handleRegister = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    navigate("/register");
  };

  const handleSortBy = (e) => {
    switch (e.target.value) {
      case "name-asc": {
        setSortBy("name-asc");
        break;
      }

      case "name-desc": {
        setSortBy("name-desc");
        break;
      }
      case "username-asc": {
        setSortBy("username-asc");
        break;
      }

      case "username-desc": {
        setSortBy("username-desc");
        break;
      }

      case "email-asc": {
        setSortBy("email-asc");
        break;
      }

      case "email-desc": {
        setSortBy("email-desc");
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

  const handleSubmitUpdate = async (updatedUser) => {
    try {
      await axiosClient.put(`/Books/${updatedUser.id}`, updatedUser);

      setUsers(users.map((u) => (u.id === updatedUser.id ? updatedUser : u)));

      notify("success", "Cập nhật người dùng thành công");
      setEditingUser(null);
    } catch (err) {
      notify("error", "Cập nhật người dùng thất bại!");
      console.error(err);
    }
  };

  const getGreetingByLocalTime = () => {
    const h = new Date().getHours();

    if (h < 10) return "Chào buổi sáng";
    if (h < 13) return "Chào buổi trưa";
    if (h < 18) return "Chào buổi chiều";
    return "Chào buổi tối";
  };

  const [weather, setWeather] = useState(null);

  useEffect(() => {
    const fetchWeather = async () => {
      try {
        const data = await fetchWeatherByLocation();
        setWeather(data);
      } catch {
        const data = await fetchWeatherByDefault();
        setWeather(data);
      }
    };

    fetchWeather();
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
  const isAdmin = user?.role === "Admin";

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
                  <span>
                    {weather.name}, {capitalize(weather.weather[0].description)}{" "}
                    {getWeatherIcon(weatherMain)},{" "}
                    {Math.round(weather.main.temp)}°C
                  </span>
                </div>
              )}
            </div>
            <div>
              {isAdmin && (
                <button
                  className="logout-btn"
                  onClick={handleBack}
                  style={{ marginRight: 10, width: "fit-content" }}
                >
                  Trang chủ
                </button>
              )}
              <button className="logout-btn" onClick={handleLogout}>
                Đăng xuất
              </button>
            </div>
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

      {isAdmin && (
        <div className="admin-section">
          <h2>Thêm thành viên mới</h2>
          <form onSubmit={addUserHandle}>
            <div className="form-group">
              <label>Tên người dùng</label>
              <input
                name="name"
                placeholder="Tên người dùng"
                value={newUser.name}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Tên tài khoản</label>
              <input
                name="username"
                placeholder="Nhập tên tài khoản"
                value={newUser.username}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Mật khẩu</label>
              <input
                type="password"
                name="password"
                placeholder="Nhập mật khẩu"
                value={newUser.password}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Nhập lại mật khẩu</label>
              <input
                type="password"
                name="verifyPassword"
                placeholder="Nhập lại mật khẩu"
                value={newUser.verifyPassword}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                name="email"
                placeholder="Nhập địa chỉ email (tuỳ chọn)"
                value={newUser.email}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-group">
              <label>Số điện thoại</label>
              <input
                type="tel"
                name="phone"
                placeholder="Nhập số điện thoại (tuỳ chọn)"
                value={newUser.phone}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-group">
              <label>Vai trò</label>
              <select
                name="role"
                value={newUser.role}
                onChange={handleInputChange}
                style={{ width: 150 }}
                required
              >
                <option value="">Chọn vai trò</option>
                <option value="Admin">Quản trị viên</option>
                <option value="User">Người dùng</option>
              </select>
            </div>

            <div className="form-group">
              <label>Ảnh đại diện</label>
              <label className="file-input-label">
                <span>{avatarFileName}</span>
                <input
                  type="file"
                  name="avatar"
                  accept="image/*"
                  onChange={handleInputChange}
                  disabled={isUploading}
                />
              </label>
              {avatarPreview && (
                <div style={{ marginTop: 10 }}>
                  <img
                    src={avatarPreview}
                    alt="Preview"
                    style={{ maxWidth: 150, maxHeight: 150, borderRadius: 8 }}
                  />
                </div>
              )}
            </div>
            <button
              type="submit"
              className="submit-btn"
              disabled={isUploading}
              style={{ opacity: isUploading ? 0.6 : 1 }}
            >
              {isUploading ? "Đang tải lên..." : "Thêm thành viên"}
            </button>
          </form>
        </div>
      )}

      <div className="main-section">
        <h1 style={{ fontWeight: "bold" }}>Quản lý thành viên</h1>

        <div className="filters">
          <div className="filter-group">
            <label>Tìm kiếm</label>
            <input
              type="text"
              placeholder="Nhập tên tên thành viên, tên tài khoản..."
              onChange={handleSearchChange}
              value={search}
              style={{ width: 300 }}
            />
          </div>
          <div className="filter-group">
            <label>Sắp xếp</label>
            <select onChange={handleSortBy} value={sortBy}>
              <option value="">Chọn sắp xếp</option>
              <option value="name-asc">Tên người dùng: A → Z</option>
              <option value="name-desc">Tên người dùng: Z → A</option>
              <option value="username-asc">Tên tài khoản: A → Z</option>
              <option value="username-desc">Tên tài khoản: Z → A</option>
              <option value="email-asc">Email: A → Z</option>
              <option value="email-desc">Email: Z → A</option>
              {/* <option value="role-asc">Vai trò: A → Z</option>
              <option value="role-desc">Vai trò: Z → A</option> */}
            </select>
          </div>
        </div>

        <table>
          <thead>
            <tr>
              <th>STT</th>
              <th>ID</th>
              <th>Tên người dùng</th>
              <th>Tên đăng nhập</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Vai trò</th>
              {isAdmin && <th>Hành động</th>}
            </tr>
          </thead>
          <tbody>
            {users.length === 0 ? (
              <>
                <tr>
                  <td
                    colSpan={isAdmin ? 6 : 5}
                    style={{ textAlign: "center", padding: 16 }}
                  >
                    Không có kết quả phù hợp
                  </td>
                </tr>

                {Array.from({ length: limit - 1 }).map((_, idx) => (
                  <tr key={`empty-${idx}`} className="empty-row">
                    <td colSpan={isAdmin ? 6 : 5}></td>
                  </tr>
                ))}
              </>
            ) : (
              <>
                {users.map((user, idx) => (
                  <tr key={user.id}>
                    <td>{idx + 1}</td>
                    <td>{user.id}</td>
                    <td>{user.name}</td>
                    <td>{user.username}</td>
                    <td>{user.email}</td>
                    <td>{user.phone}</td>
                    <td>{user.role}</td>
                    {isAdmin && (
                      <td style={{ textAlign: "center" }}>
                        <div className="action-buttons">
                          <button className="edit-btn">Sửa</button>
                          <button className="delete-btn">Xóa</button>
                        </div>
                      </td>
                    )}
                  </tr>
                ))}
                {Array.from({ length: 10 - users.length }).map((_, idx) => (
                  <tr key={`empty-${idx}`} className="empty-row">
                    <td colSpan={isAdmin ? 6 : 5}></td>
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

      {editingUser && (
        <EditBookModal
          user={editingUser}
          onClose={() => setEditingUser(null)}
          onSubmit={handleSubmitUpdate}
        />
      )}
    </div>
  );
};

export default UserPage;
