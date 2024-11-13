import React from "react";
import "../css/JobItem.css";
import { JobItemProps } from "../types/JobItemProps";

const JobItem: React.FC<JobItemProps> = ({
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
        <a href="/edit" className=" edit-button apply-button">
          Edit Listing
        </a>
        <a href="/delete" className="apply-button">
          Delete Listing
        </a>
      </div>
    </div>
  );
};

export default JobItem;
