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
        // Decode JWT token to extract email
        const decodedToken = JSON.parse(atob(token.split(".")[1])); // Decoding the payload part of the JWT
        console.log("Decoded Token:", decodedToken); // Debug: log the decoded token
        setUserEmail(
          decodedToken?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
          ] || "Unknown User"
        ); // Access the email from the correct key
      } catch (error) {
        console.error("Error decoding token:", error);
        setUserEmail("Unknown User"); // Fallback in case decoding fails
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
            <span>{userEmail}</span> {/* Display the logged-in user's email */}
            <span className="separator">|</span>
            <Link to="#" onClick={logout}>
              Logout
            </Link>{" "}
            {/* Logout button */}
          </>
        ) : (
          <>
            <Link to="/signup">Signup</Link>
            <span className="separator">|</span>
            <Link to="/login">Login</Link>
          </>
        )}
      </div>
    </header>
  );
};

export default Navbar;
