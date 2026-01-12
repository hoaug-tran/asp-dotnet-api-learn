const Foot = () => {
  const foots = ["orange", "banana"];

  return (
    <ul>
      <li>apple</li>
      <li>{foots[0]}</li>
      <li>{foots[1].toUpperCase()}</li>
    </ul>
  );
};

export default Foot;
