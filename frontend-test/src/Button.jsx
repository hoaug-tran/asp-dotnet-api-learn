const Button = () => {
  //   const handleClick = () => {
  //     console.log("OUCH!");
  //   };

  //   let count = 0;

  //   const handleClick2 = (name) => {
  //     if (count < 3) {
  //       count++;
  //       console.log(`${name} you clicked me ${count} times`);
  //     } else {
  //       console.log(`${name} stop clicking me`);
  //     }
  //   };

  const handleClick = (e) => {
    e.target.textContent = "SUCK";
  };

  return <button onDoubleClick={(e) => handleClick(e)}>Click me</button>;
};

export default Button;
