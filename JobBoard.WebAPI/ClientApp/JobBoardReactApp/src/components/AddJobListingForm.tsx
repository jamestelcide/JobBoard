import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { JobItemProps } from "../types/JobItemProps";
import { JobTypeOptions } from "../types/JobTypeOptions";
import "../css/JobListingForm.css";
import { useAuth } from "../utils/AuthContext";

const AddJobListingForm: React.FC = () => {
  const [job, setJob] = useState<JobItemProps>({
    jobID: "",
    jobTitle: "",
    companyName: "",
    email: "",
    cityAndState: "",
    payRange: "",
    jobType: JobTypeOptions.FullTime,
    jobPostedDate: new Date(),
    fullDescription: "",
  });

  const { getToken } = useAuth();
  const navigate = useNavigate();

  const handleInputChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    const { name, value } = e.target;
    setJob((prevJob) => ({
      ...prevJob,
      [name]: name === "jobPostedDate" ? new Date(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = getToken();
    try {
      const updatedJob = {
        ...job,
        jobType: parseInt(job.jobType.toString(), 10),
        jobPostedDate: job.jobPostedDate.toISOString(),
      };
      console.log("Sending job data:", updatedJob);

      const response = await axios.post(
        "https://localhost:7181/api/joblisting",
        updatedJob,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      console.log("Job submitted:", response.data);
      setJob({
        jobID: "",
        jobTitle: "",
        companyName: "",
        email: "",
        cityAndState: "",
        payRange: "",
        jobType: JobTypeOptions.FullTime,
        jobPostedDate: new Date(),
        fullDescription: "",
      });
      navigate("/");
    } catch (error) {
      console.error("Error submitting job:", error);
    }
  };

  return (
    <div className="job-form-container">
      <h2 className="form-title">Add New Job Listing</h2>
      <form onSubmit={handleSubmit} className="job-form">
        <label className="form-label">
          Job Title:
          <input
            type="text"
            name="jobTitle"
            value={job.jobTitle}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Company Name:
          <input
            type="text"
            name="companyName"
            value={job.companyName}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Email:
          <input
            type="email"
            name="email"
            value={job.email}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          City and State:
          <input
            type="text"
            name="cityAndState"
            value={job.cityAndState}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Pay Range:
          <input
            type="text"
            name="payRange"
            value={job.payRange}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Job Type:
          <select
            name="jobType"
            value={job.jobType}
            onChange={handleInputChange}
            className="form-input"
            required
          >
            <option value={JobTypeOptions.FullTime}>FullTime</option>
            <option value={JobTypeOptions.PartTime}>PartTime</option>
            <option value={JobTypeOptions.Internship}>Internship</option>
            <option value={JobTypeOptions.Remote}>Remote</option>
          </select>
        </label>

        <label className="form-label">
          Job Posted Date:
          <input
            type="date"
            name="jobPostedDate"
            value={job.jobPostedDate.toISOString().substring(0, 10)} //Formats the date as yyyy-MM-dd
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </label>

        <label className="form-label">
          Full Description:
          <textarea
            name="fullDescription"
            value={job.fullDescription}
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
