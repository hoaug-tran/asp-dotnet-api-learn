import { useEffect, useRef } from "react";

const MyComponent = () => {
  const ref = useRef(0);

  useEffect(() => {
    console.log("COMPONENT RENDERED");
  });

  const handleClick = () => {
    ref.current++;
    console.log(ref.current);
  };

  return <button onClick={handleClick}>Click tao</button>;
};

export default MyComponent;
