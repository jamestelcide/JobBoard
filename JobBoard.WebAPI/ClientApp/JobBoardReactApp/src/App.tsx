import React from "react";
import Home from "./pages/Home";
import AddJobListing from "./pages/AddJobListingPage";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./css/App.css";
import EditJobListing from "./pages/EditJobListingPage";
import DeleteJobListing from "./pages/DeleteJobListingPage";
import LogoutPage from "./pages/LogoutPage";
import LoginPage from "./pages/LogInPage";
import SignUp from "./pages/SignUp";

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
        <Route path="/edit/:jobID" element={<EditJobListing />} />
        <Route path="/delete/:jobID" element={<DeleteJobListing />} />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/logout" element={<LogoutPage />} />
      </Routes>
    </Router>
  );
};

export default App;
