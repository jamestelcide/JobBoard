import React from "react";
import "../css/HeroSection.css";

const HeroSection: React.FC = () => {
  return (
    <div className="hero-container">
      <h1 className="hero-title">
        Join a network to list, edit, and create impact within the hiring
        market!
      </h1>
      <h2 className="hero-subtitle">
        Our philosophy is simple — hire a team of diverse, passionate people and
        foster a culture that empowers you to do your best work.
      </h2>
      <div className="hero-shapes">
        <div className="shape cube"></div>
        <div className="shape semicircle"></div>
        <div className="shape cylinder"></div>
        <div className="shape circle"></div>
      </div>
      <div className="briefcase">
        <div className="handle"></div>
        <div className="lock left"></div>
        <div className="lock right"></div>
      </div>
      <div className="hero-shapes">
        <div className="shape circle"></div>
        <div className="shape cube"></div>
        <div className="shape semicircle"></div>
        <div className="shape cylinder"></div>
      </div>
    </div>
  );
};

export default HeroSection;
