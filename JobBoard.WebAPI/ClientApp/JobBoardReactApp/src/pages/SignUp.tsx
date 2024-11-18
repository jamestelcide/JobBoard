import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import axios from "axios";
import "../css/SignUp.css";

const SignUp: React.FC = () => {
  const [personName, setPersonName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null); // Reset error before submitting

    if (password !== confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    try {
      const response = await axios.post(
        "https://localhost:7181/api/account/register",
        {
          PersonName: personName,
          Email: email,
          PhoneNumber: phoneNumber,
          Password: password,
          confirmPassword: confirmPassword
        },
        {
          headers: {
            "Content-Type": "application/json",
            Accept: "application/json",
          },
        }
      );

      if (response.status === 200) {
        console.log("Registration successful:", response.data);
        navigate("/login"); // Redirect to login page after successful signup
      } else {
        setError("An error occurred. Please try again.");
      }
    } catch (err: any) {
      console.error("Signup error:", err);
      setError(err.response?.data?.message || "An error occurred during registration.");
    }
  };

  return (
    <div className="signup-container" style={{ textAlign: "center", marginTop: "50px" }}>
      <h1>Create an Account</h1>
      {error && <p className="error-message">{error}</p>}
      <form className="signup-form" onSubmit={handleSubmit}>
        <div>
          <label htmlFor="personName">Name:</label>
          <input
            type="text"
            id="personName"
            value={personName}
            onChange={(e) => setPersonName(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="phoneNumber">Phone Number:</label>
          <input
            type="text"
            id="phoneNumber"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
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
        <div>
          <label htmlFor="confirmPassword">Confirm Password:</label>
          <input
            type="password"
            id="confirmPassword"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="signup-button">
          Sign Up
        </button>
      </form>
      <p>
        Already have an account?{" "}
        <Link to="/login" className="signup-login-link">
          Log in
        </Link>
      </p>
    </div>
  );
};

export default SignUp;
