import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import "../css/LoginPage.css"; // Optional, for custom styling if needed

const LoginPage: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();

    // Placeholder login logic (replace with actual authentication logic as needed)
    if (username && password) {
      console.log("Logging in with:", { username, password });
      navigate("/");
    } else {
      alert("Please enter both username and password");
    }
  };

  return (
    <div className="login-container" style={{ textAlign: "center", marginTop: "50px" }}>
      <h1>Login</h1>
      <form onSubmit={handleLogin} className="login-form">
        <div>
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="login-button">Log In</button>
      </form>

      <p>
        Don't have an account?{" "}
        <Link to="/signup" className="signup-link">
          Sign up
        </Link>
      </p>
    </div>
  );
};

export default LoginPage;