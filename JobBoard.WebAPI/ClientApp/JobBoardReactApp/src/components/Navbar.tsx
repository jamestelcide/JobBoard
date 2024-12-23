import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../utils/AuthContext";
import "../css/Navbar.css";

const Navbar: React.FC = () => {
  const { isLoggedIn, logout, getToken } = useAuth();
  const [userEmail, setUserEmail] = useState<string | null>(null);

  useEffect(() => {
    const token = getToken();
    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split(".")[1]));
        console.log("Decoded Token:", decodedToken);
        setUserEmail(
          decodedToken?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
          ] || "Unknown User"
        );
      } catch (error) {
        console.error("Error decoding token:", error);
        setUserEmail("Unknown User");
      }
    }
  }, [getToken]);

  return (
    <header className="nav">
      <div className="nav-left">
        <img src="/logo-upscaled.png" className="logo" alt="logo" />
        <nav className="nav-links">
          <Link to="/">Home</Link>
          <Link to="/add">Post Job Listing</Link>
        </nav>
      </div>
      <div className="nav-right">
        {isLoggedIn ? (
          <>
            <span className="user-email">{userEmail}</span> 
            <Link to="#" onClick={logout} className="nav-btn">
              Logout
            </Link>{" "}
          </>
        ) : (
          <>
            <Link to="/signup" className="nav-btn">Signup</Link>
            <Link to="/login" className="nav-btn">Login</Link>
          </>
        )}
      </div>
    </header>
  );
};

export default Navbar;
