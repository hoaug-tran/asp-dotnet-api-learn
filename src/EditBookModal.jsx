import { useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";

const EditBookModal = ({ book, onClose, onSubmit }) => {
  const [form, setForm] = useState({
    title: book.title,
    author: book.author,
    price: book.price,
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
    console.log(form);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

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

    if (!window.confirm("Bạn có chắc muốn sửa?")) {
      return;
    }

    onSubmit({
      ...book,
      ...form,
    });
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal-header">
          <h2>Sửa sách</h2>
          <button className="modal-close" onClick={onClose}>
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Tên sách</label>
            <input
              name="title"
              value={form.title}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label>Tác giả</label>
            <input
              name="author"
              value={form.author}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label>Giá</label>
            <input
              type="number"
              name="price"
              value={form.price}
              onChange={handleChange}
              required
            />
          </div>

          <div className="modal-actions">
            <button type="button" className="cancel-btn" onClick={onClose}>
              Huỷ
            </button>
            <button type="submit" className="submit-btn">
              Lưu
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditBookModal;
