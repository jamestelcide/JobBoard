import React from "react";
import { Link } from "react-router-dom";
import "../css/Navbar.css";

const Navbar: React.FC = () => {
  return (
    <header className="nav">
      <div className="nav-left">
        <img src="/logo-upscaled.png" className="logo" />
        <nav className="nav-links">
          <Link to="/">Home</Link>
          <Link to="/add">Post Job Listing</Link>
          <Link to="/">Current Company Listings</Link>
        </nav>
      </div>
      <div className="nav-right">
        <Link to="/">Sign in</Link>
        <span className="separator">|</span>
        <Link to="/">Logout</Link>
      </div>
    </header>
  );
};

export default Navbar;
