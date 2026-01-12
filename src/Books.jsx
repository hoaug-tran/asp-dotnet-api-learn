// import { useState, useEffect } from "react";
import axios from "axios";
import { useQuery } from "@tanstack/react-query";

function Books() {
  //   const [books, setBooks] = useState([]);
  //   const [error, setError] = useState(null);

  //   useEffect(() => {
  //     fetch("https://api.libsys.me/api/v1/books")
  //       .then((res) => {
  //         if (!res.ok) throw new Error("fetch fail");
  //         return res.json();
  //       })
  //       .then((json) => {
  //         setBooks(json.data);
  //       })
  //       .catch((err) => {
  //         setError(err.message);
  //       });
  //   }, []);

  //   if (error) return <p>Error: {error}</p>;

  //   useEffect(() => {
  //     const bookF = async () => {
  //       const res = await axios.get("https://api.libsys.me/api/v1/books");
  //       setBooks(res.data.data);
  //     };

  //     bookF();
  //   }, []);

  //   if (error) return <p>Error: {error}</p>;

  const fetchBooks = async () => {
    const res = await axios.get(
      "https://localhost:7216/api/v1/Books?page=1&limit=10&title=&sortBy=author&order=desc"
    );
    return res.data.data;
  };

  const { data, isLoading, isError, error } = useQuery({
    queryKey: ["books"],
    queryFn: fetchBooks,
  });

  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (isError) {
    return <p>{error.message}</p>;
  }

  return (
    <ul>
      {data.map((b) => (
        <li key={b.id}>{b.title}</li>
      ))}
    </ul>
  );
}

export default Books;
