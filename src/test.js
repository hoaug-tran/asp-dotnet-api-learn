import axios from "axios";

const res = await axios.get("https://localhost:7216/api/v1/Books");
const { items } = res.data.data;

console.log(items);
