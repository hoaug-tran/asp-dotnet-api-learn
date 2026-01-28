import { useEffect, useState, useRef } from "react";
import "../../style/Toast.css";

const NotificationToast = ({ notifications = [] }) => {
  const [visibleNotis, setVisibleNotis] = useState([]);
  const timersRef = useRef({});

  useEffect(() => {
    if (notifications.length === 0) {
      return;
    }

    const lastNoti = notifications[notifications.length - 1];
    const notiId = lastNoti.id || Date.now();

    // eslint-disable-next-line react-hooks/set-state-in-effect
    setVisibleNotis((prev) => {
      const exists = prev.some((n) => n.id === notiId);
      if (exists) return prev;
      return [...prev, { ...lastNoti, id: notiId }];
    });

    if (timersRef.current[notiId]) {
      clearTimeout(timersRef.current[notiId]);
    }

    timersRef.current[notiId] = setTimeout(() => {
      setVisibleNotis((prev) => prev.filter((n) => n.id !== notiId));
      delete timersRef.current[notiId];
    }, 3000);
  }, [notifications]);

  return (
    <div className="toast-container">
      {visibleNotis.map((noti) => (
        <div key={noti.id} className={`toast toast-${noti.type}`}>
          <div className="toast-icon">
            {noti.type === "success" && "✓"}
            {noti.type === "error" && "✕"}
            {noti.type === "warning" && "⚠"}
            {noti.type === "info" && "ℹ"}
          </div>
          <div className="toast-content">
            <p className="toast-message">{noti.message}</p>
          </div>
          <button
            className="toast-close"
            onClick={() =>
              setVisibleNotis((prev) => prev.filter((n) => n.id !== noti.id))
            }
          >
            ×
          </button>
        </div>
      ))}
    </div>
  );
};

export default NotificationToast;
