import React from "react";
import Navbar from "../components/Navbar";
import EditJobListingForm from "../components/EditJobListingForm";

const EditJobListingPage: React.FC = () => {
  return (
    <div>
      <Navbar />
      <EditJobListingForm />
    </div>
  );
};

export default EditJobListingPage;
