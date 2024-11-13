import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/SignUp.css";

const SignUp: React.FC = () => {
  const [formData, setFormData] = useState({
    personName: "",
    email: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
  });

  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (formData.password === formData.confirmPassword) {
      console.log("Form submitted successfully:", formData);
      navigate("/");
    } else {
      alert("Passwords do not match");
    }
  };

  return (
    <div className="signup-container">
      <h2 className="signup-title">Create an Account</h2>
      <form className="signup-form" onSubmit={handleSubmit}>
        <label>
          Name:
          <input
            type="text"
            name="personName"
            value={formData.personName}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Email:
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Phone Number:
          <input
            type="text"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Password:
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Confirm Password:
          <input
            type="password"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            required
          />
        </label>

        <button type="submit" className="signup-button">
          Sign Up
        </button>
      </form>
      <p className="signup-login-link">
        Already have an account? <a href="/login">Log in</a>
      </p>
    </div>
  );
};

export default SignUp;
