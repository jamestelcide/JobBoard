import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate, useParams } from "react-router-dom";
import { JobItemProps } from "../types/JobItemProps";
import { JobTypeOptions } from "../types/JobTypeOptions";
import { useAuth } from "../utils/AuthContext";
import "../css/JobListingForm.css";

const DeleteJobListingForm: React.FC = () => {
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

  const navigate = useNavigate();
  const { jobID } = useParams<{ jobID: string }>();
  const { getToken } = useAuth();

  useEffect(() => {
    const fetchJobDetails = async () => {
      const token = getToken();
      try {
        const response = await axios.get(
          `https://localhost:7181/api/joblisting/id/${jobID}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setJob({
          ...response.data,
          jobPostedDate: new Date(response.data.jobPostedDate),
        });
      } catch (error) {
        console.error("Error fetching job details:", error);
      }
    };
    fetchJobDetails();
  }, [jobID, getToken]);
  
  const handleDelete = async () => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this job listing?"
    );
    if (confirmDelete) {
      const token = getToken();
      try {
        await axios.delete(`https://localhost:7181/api/joblisting/${jobID}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        console.log("Job deleted:", jobID);
        navigate("/");
      } catch (error) {
        console.error("Error deleting job:", error);
      }
    }
  };

  return (
    <div className="job-form-container">
      <h2 className="form-title">
        Are you sure you want to delete this Job Listing?
      </h2>
      <form className="job-form">
        <label className="form-label">
          Job Title:
          <input
            type="text"
            name="jobTitle"
            value={job.jobTitle}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Company Name:
          <input
            type="text"
            name="companyName"
            value={job.companyName}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Email:
          <input
            type="email"
            name="email"
            value={job.email}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          City and State:
          <input
            type="text"
            name="cityAndState"
            value={job.cityAndState}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Pay Range:
          <input
            type="text"
            name="payRange"
            value={job.payRange}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Job Type:
          <input
            type="text"
            name="jobType"
            value={JobTypeOptions[job.jobType]}
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Job Posted Date:
          <input
            type="date"
            name="jobPostedDate"
            value={job.jobPostedDate.toISOString().split("T")[0]} //Formats the date as yyyy-MM-dd
            readOnly
            className="form-input"
          />
        </label>

        <label className="form-label">
          Full Description:
          <textarea
            name="fullDescription"
            value={job.fullDescription}
            readOnly
            className="form-textarea"
          />
        </label>

        <button type="button" className="submit-button" onClick={handleDelete}>
          Delete Job
        </button>
      </form>
    </div>
  );
};

export default DeleteJobListingForm;
