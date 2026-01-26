import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./LoginPage.css";
import axiosClient from "./axiosClient";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
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
      const res = await axiosClient.post("/Auth/login", {
        username: username,
        password: password,
      });
      const { accessToken, user } = res.data;
      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("user", JSON.stringify(user));

      alert("Đăng nhập thành công");
      navigate("/");
    } catch (err) {
      alert("Tên đăng nhập hoặc mật khẩu không đúng");
      setPassword("");
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
        <h1>Đăng Nhập</h1>
        <form onSubmit={handleLogin}>
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
          <p style={{ display: "inline-block", fontSize: "0.9rem" }}>
            Chưa có tài khoản ?{" "}
            <span>
              <a
                style={{ textDecoration: "none", color: "blue" }}
                href="/register"
              >
                Đăng ký
              </a>{" "}
              ngay.
            </span>
          </p>
          <button type="submit" className="login-btn" disabled={loading}>
            {loading ? "Đang đăng nhập..." : "Đăng Nhập"}
          </button>
        </form>
        <button
          type="submit"
          className="back-btn"
          onClick={(e) => handleBackHome(e)}
        >
          Quay về trang chủ
        </button>
      </div>
    </div>
  );
};

export default LoginPage;
