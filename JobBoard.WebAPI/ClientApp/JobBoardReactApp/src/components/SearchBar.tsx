import React, { useState } from "react";
import "../css/SearchBar.css";

interface SearchBarProps {
  onSearch: (name: string, location: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch }) => {
  const [jobTitle, setJobTitle] = useState<string>("");
  const [location, setLocation] = useState<string>("");

  const handleSearch = () => {
    onSearch(jobTitle, location);
  };

  return (
    <div className="container">
      <div className="search-bar">
        <input
          type="text"
          className="search-input"
          placeholder="Job Title"
          value={jobTitle}
          onChange={(e) => setJobTitle(e.target.value)}
        />
        <span className="location-divider">|</span>
        <input
          type="text"
          className="location-input"
          placeholder="Location? example: Atlanta, GA"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
        />
        <button className="search-button" onClick={handleSearch}>
          Search
        </button>
        <div className="post-listing">
          <a href="/add">Create Job Listing</a> - It only takes a few seconds
        </div>
      </div>
    </div>
  );
};

export default SearchBar;
