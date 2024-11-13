import React from "react";
import { Link } from "react-router-dom";
import "../css/JobItem.css";
import { JobItemProps } from "../types/JobItemProps";

const JobItem: React.FC<JobItemProps> = ({
  jobID,
  jobTitle,
  companyName,
  email,
  cityAndState,
  payRange,
  jobType,
  jobPostedDate,
  fullDescription,
}) => {
  return (
    <div className="job-item">
      <h2 className="job-title">{jobTitle}</h2>
      <p className="job-company">{companyName}</p>
      <p className="job-location">{email}</p>
      <p className="job-location">{cityAndState}</p>
      <p className="job-type">{payRange}</p>
      <p className="job-type">{jobType}</p>
      <p className="job-type">{new Date(jobPostedDate).toLocaleDateString()}</p>
      <p className="job-type">{fullDescription}</p>
      <a href="#" className="apply-button">
        Apply On Company Website
      </a>
      <br></br>
      <div>
        <Link to={`/edit/${jobID}`} className="edit-button apply-button">
          Edit Listing
        </Link>
        <a href="/delete" className="apply-button">
          Delete Listing
        </a>
      </div>
    </div>
  );
};

export default JobItem;
