// const Student = (props) => {
//   return (
//     <div>
//       <p>Name: {props.name}</p>
//       <p>Age: {props.age}</p>
//       <p>Student: {props.isStudent ? "YES" : "NO"}</p>
//     </div>
//   );
// };

// import PropTypes from "prop-types";

// Student.propTypes = {
//   name: PropTypes.string,
//   age: PropTypes.number,
//   isStudent: PropTypes.bool,
// };

const Student = ({ name, age, isStudent }) => {
  return (
    <div>
      <p>Name: {name}</p>
      <p>Age: {age}</p>
      <p>Student: {isStudent ? "YES" : "NO"}</p>
    </div>
  );
};

export default Student;
