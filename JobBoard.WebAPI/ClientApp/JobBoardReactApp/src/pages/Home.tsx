import React from "react";
import JobList from "../components/JobList";
import Navbar from "../components/Navbar";
import HeroSection from "../components/HeroSection";

const Home: React.FC = () => {
  return (
    <div>
      <Navbar />
      <HeroSection />
      <JobList />
    </div>
  );
};

export default Home;
