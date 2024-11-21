import React from "react";
import JobList from "../components/JobList";
import Navbar from "../components/Navbar";

const Home: React.FC = () => {
  return (
    <div>
      <Navbar />
      <JobList />
    </div>
  );
};

export default Home;
