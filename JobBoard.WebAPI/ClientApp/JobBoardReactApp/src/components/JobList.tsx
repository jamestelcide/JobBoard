import React, { useEffect, useState } from "react";
import axios from "axios";
import JobItem from "./JobItem";
import "../css/JobList.css";
import { JobItemProps } from "../types/JobItemProps";
import { useAuth } from "../utils/AuthContext";

const JobList: React.FC = () => {
  const [jobs, setJobs] = useState<JobItemProps[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const { getToken } = useAuth(); // Access the token

  useEffect(() => {
    const fetchJobs = async () => {
      const token = getToken();
      try {
        const response = await axios.get<JobItemProps[]>(
          "https://localhost:7181/api/joblisting",
          {
            headers: {
              Authorization: `Bearer ${token}`, // Attach token to API request
            },
          }
        );
        setJobs(response.data);
      } catch (error) {
        console.error("Error fetching jobs:", error);
        setError(
          "Failed to load job listings. Please log in if you have not already."
        );
      } finally {
        setLoading(false);
      }
    };

    fetchJobs();
  }, [getToken]); // Re-fetch if the token changes

  if (loading) return <h3>Loading...</h3>;
  if (error) return <p>{error}</p>;

  return (
    <div className="job-list">
      {jobs.map((job) => (
        <JobItem key={job.jobID} {...job} />
      ))}
    </div>
  );
};

export default JobList;
