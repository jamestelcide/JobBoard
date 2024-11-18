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
import ProtectedRoute from "./utils/ProtectedRoute";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route
          path="/add"
          element={
            <ProtectedRoute>
              <AddJobListing />
            </ProtectedRoute>
          }
        />
        <Route
          path="/edit/:jobID"
          element={
            <ProtectedRoute>
              <EditJobListing />
            </ProtectedRoute>
          }
        />
        <Route
          path="/delete/:jobID"
          element={
            <ProtectedRoute>
              <DeleteJobListing />
            </ProtectedRoute>
          }
        />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/logout"
          element={
            <ProtectedRoute>
              <LogoutPage />
            </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
};

export default App;
