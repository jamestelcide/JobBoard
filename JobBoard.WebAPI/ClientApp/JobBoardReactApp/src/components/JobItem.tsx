import React from "react";
import "../css/JobItem.css";
import { JobItemProps } from "../types/JobItemProps";

const JobItem: React.FC<JobItemProps> = ({
  JobTitle,
  CompanyName,
  Email,
  CityAndState,
  PayRange,
  jobType,
  JobPostedDate,
  FullDescription,
}) => {
  return (
    <div className="job-item">
      <h2 className="job-title">{JobTitle}</h2>
      <p className="job-company">{CompanyName}</p>
      <p className="job-location">{Email}</p>
      <p className="job-location">{CityAndState}</p>
      <p className="job-type">{PayRange}</p>
      <p className="job-type">{jobType}</p>
      <p className="job-type">{JobPostedDate.toDateString()}</p>
      <p className="job-type">{FullDescription}</p>
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
