import React from "react";
import Navbar from "../components/Navbar";
import AddJobListingForm from "../components/AddJobListingForm";

const AddJobListingPage: React.FC = () => {
  return (
    <div>
      <Navbar />
      <AddJobListingForm />
    </div>
  );
};

export default AddJobListingPage;
