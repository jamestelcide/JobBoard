import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../utils/AuthContext";

const LogoutPage: React.FC = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    logout(); // Clear the token and auth state
    navigate("/login"); // Redirect to login page
  }, [logout, navigate]);

  return <p>Logging out...</p>;
};

export default LogoutPage;
