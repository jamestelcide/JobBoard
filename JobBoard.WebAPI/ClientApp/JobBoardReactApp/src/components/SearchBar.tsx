import React from "react";
import "../css/SearchBar.css";

const SearchBar: React.FC = () => {
  return (
    <div className="container">
      <div className="search-bar">
        <input
          type="text"
          className="search-input"
          placeholder="Job title, company, etc."
        />
        <span className="location-divider">|</span>
        <input
          type="text"
          className="location-input"
          placeholder="Location? example: Atlanta, GA"
        />
        <button className="search-button">Search</button>
        <div className="post-listing">
          <a href="#">Create Job Listing</a> - It only takes a few seconds
        </div>
      </div>
    </div>
  );
};

export default SearchBar;
