import React from "react";
import Home from "./pages/Home";
import AddJobListing from "./pages/AddJobListingPage";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./css/App.css";
import EditJobListing from "./pages/EditJobListingPage";
import DeleteJobListing from "./pages/DeleteJobListingPage";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={
            <div className="App">
              <Home />
            </div>
          }
        />
        <Route path="/add" element={<AddJobListing />} />
        <Route path="/edit" element={<EditJobListing />} />
        <Route path="/delete" element={<DeleteJobListing />} />
      </Routes>
    </Router>
  );
};

export default App;
