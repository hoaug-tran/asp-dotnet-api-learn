import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./LoginPage.css";
import axiosClient from "./axiosClient";

const LoginPage = () => {
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [verifyPassword, setVerifyPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    if (localStorage.getItem("accessToken")) {
      navigate("/");
    }
  }, []);

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await axiosClient.post("/Auth/register", {
        name: name,
        username: username,
        password: password,
        verifyPassword: verifyPassword,
      });

      alert("Đăng ký thành công");
      navigate("/login");
    } catch (err) {
      const msg =
        err.response?.data?.message || err.response?.data || "Đăng ký thất bại";

      alert(msg);
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleBackHome = (e) => {
    e.preventDefault();
    navigate("/");
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Đăng Ký</h1>
        <form onSubmit={handleLogin}>
          <div className="form-group">
            <label>Tên người dùng</label>
            <input
              type="text"
              name="name"
              placeholder="Nhập tên người dùng"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Tên đăng nhập</label>
            <input
              type="text"
              name="username"
              placeholder="Nhập tên đăng nhập"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Mật khẩu</label>
            <input
              type="password"
              name="password"
              placeholder="Nhập mật khẩu"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Nhập lại khẩu</label>
            <input
              type="password"
              name="verifyPassword"
              placeholder="Nhập mật lại khẩu"
              value={verifyPassword}
              onChange={(e) => setVerifyPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="login-btn" disabled={loading}>
            {loading ? "Đang thực hiện..." : "Đăng ký"}
          </button>
        </form>
        <button
          type="submit"
          className="login-btn"
          onClick={(e) => handleBackHome(e)}
        >
          Quay về trang chủ
        </button>
      </div>
    </div>
  );
};

export default LoginPage;
