const List = () => {
  const fruits = ["apple", "orange", "banana", "coconut", "pineapple"];

  const listItems = fruits.map((f) => <li>{f}</li>);

  return <ul>{listItems}</ul>;
};

export default List;
