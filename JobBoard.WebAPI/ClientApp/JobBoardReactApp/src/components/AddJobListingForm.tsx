import React, { useState } from "react";
import { JobItemProps } from "../types/JobItemProps";
import "../css/JobListingForm.css";

const AddJobListingForm: React.FC = () => {
  const [job, setJob] = useState<JobItemProps>({
    JobID: "",
    JobTitle: "",
    CompanyName: "",
    Email: "",
    CityAndState: "",
    PayRange: "",
    jobType: "",
    JobPostedDate: new Date(),
    FullDescription: "",
  });

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setJob((prevJob) => ({
      ...prevJob,
      [name]: name === "JobPostedDate" ? new Date(value) : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Job submitted:", job);
    // Add form submission logic here if needed
  };

  return (
    <div className="job-form-container">
      <h2 className="form-title">Add New Job Listing</h2>
      <form onSubmit={handleSubmit} className="job-form">
        <label className="form-label">
          Job Title:
          <input
            type="text"
            name="JobTitle"
            value={job.JobTitle}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Company Name:
          <input
            type="text"
            name="CompanyName"
            value={job.CompanyName}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Email:
          <input
            type="email"
            name="Email"
            value={job.Email}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          City and State:
          <input
            type="text"
            name="CityAndState"
            value={job.CityAndState}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Pay Range:
          <input
            type="text"
            name="PayRange"
            value={job.PayRange}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Job Type:
          <input
            type="text"
            name="jobType"
            value={job.jobType}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Job Posted Date:
          <input
            type="date"
            name="JobPostedDate"
            value={job.JobPostedDate.toISOString().substring(0, 10)}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Full Description:
          <textarea
            name="FullDescription"
            value={job.FullDescription}
            onChange={handleInputChange}
            className="form-textarea"
            required
          />
        </label>

        <button type="submit" className="submit-button">
          Submit Job
        </button>
      </form>
    </div>
  );
};

export default AddJobListingForm;
