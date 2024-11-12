import React from "react";
import Navbar from "../components/Navbar";
import DeleteJobListingForm from "../components/DeleteJobListingForm";

const DeleteJobListingPage: React.FC = () => {
  return (
    <div>
      <Navbar />
      <DeleteJobListingForm />
    </div>
  );
};

export default DeleteJobListingPage;
