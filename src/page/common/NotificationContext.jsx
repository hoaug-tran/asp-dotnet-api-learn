import { createContext, useState } from "react";
import NotificationToast from "./NotificationToast";

const NotificationContext = createContext();

const NotificationProvider = ({ child }) => {
  const [noti, setNoti] = useState([]);

  const notify = (type, message) => {
    setNoti((n) => [...n, { type, message }]);
  };

  return (
    <>
      <NotificationContext.Provider value={{ notify }}>
        {child}
        <NotificationToast notifications={noti}></NotificationToast>
      </NotificationContext.Provider>
    </>
  );
};

export { NotificationContext, NotificationProvider };
export default NotificationProvider;
