import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7216/api/v1/Auth/login", {
        username: username,
        password: password,
      });
      localStorage.setItem("user", JSON.stringify(res.data));
      alert("Đăng nhập thành công");
      navigate("/");
    } catch (err) {
      alert("Tên đăng nhập hoặc tài khoản không đúng");
      console.error(err);
    }
  };

  return (
    <>
      <h1>Loginnnnnnnnnnnnn</h1>
      <form onSubmit={handleLogin}>
        <label>Tên đăng nhập: </label>
        <input
          type="text"
          name="username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        ></input>
        <br></br>
        <label>Mật khẩu: </label>
        <input
          type="password"
          name="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        ></input>
        <br></br>
        <input type="submit" value="Đăng nhập"></input>
      </form>
    </>
  );
};

export default LoginPage;
