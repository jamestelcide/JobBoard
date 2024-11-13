import React, { useEffect, useState } from "react";
import axios from "axios";
import JobItem from "./JobItem";
import "../css/JobList.css";
import { JobItemProps } from "../types/JobItemProps";

const JobList: React.FC = () => {
  const [jobs, setJobs] = useState<JobItemProps[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        const response = await axios.get<JobItemProps[]>(
          "https://localhost:7181/api/joblisting"
        );
        setJobs(response.data);
      } catch (error) {
        setError("Failed to load job listings.");
      } finally {
        setLoading(false);
      }
    };

    fetchJobs();
  }, []);

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
