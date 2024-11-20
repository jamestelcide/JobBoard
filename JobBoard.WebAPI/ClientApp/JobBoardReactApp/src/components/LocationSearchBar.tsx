import React, { useState } from "react";
import "../css/SearchBar.css";

interface SearchBarProps {
  onSearch: (location: string) => void;
}

const LocationSearchBar: React.FC<SearchBarProps> = ({ onSearch }) => {
  const [location, setLocation] = useState<string>("");

  const handleSearch = () => {
    onSearch(location);
  };

  return (
    <div className="container">
      <div className="search-bar">
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

export default LocationSearchBar;
