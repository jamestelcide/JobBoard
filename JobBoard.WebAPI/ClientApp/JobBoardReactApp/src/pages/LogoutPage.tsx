import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/LogoutPage.css";

const LogoutPage: React.FC = () => {
  const [countdown, setCountdown] = useState(5);
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setInterval(() => {
      setCountdown((prevCountdown) => prevCountdown - 1);
    }, 1000);

    if (countdown === 0) {
      navigate("/");
    }

    return () => clearInterval(timer);
  }, [countdown, navigate]);

  return (
    <div className="logout-container">
      <h1>You have logged out successfully.</h1>
      <p>
        Redirecting to the home page in <span>{countdown}</span> seconds...
      </p>
    </div>
  );
};

export default LogoutPage;
