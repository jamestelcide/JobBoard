import React from "react";
import JobList from "../components/JobList";
import Navbar from "../components/Navbar";
import SearchBar from "../components/SearchBar";

const Home: React.FC = () => {
  return (
    <div>
      <Navbar />
      <SearchBar />
      <JobList />
    </div>
  );
};

export default Home;
