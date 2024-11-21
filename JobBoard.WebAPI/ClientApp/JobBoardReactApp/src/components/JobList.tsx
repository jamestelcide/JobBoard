import React, { useEffect, useState } from "react";
import axios from "axios";
import JobItem from "./JobItem";
import "../css/JobList.css";
import { JobItemProps } from "../types/JobItemProps";
import { useAuth } from "../utils/AuthContext";
import LocationSearchBar from "./LocationSearchBar";

const JobList: React.FC = () => {
  const [jobs, setJobs] = useState<JobItemProps[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const { getToken } = useAuth();

  const fetchJobs = async (location?: string) => {
    const token = getToken();
    setLoading(true);

    try {
      const endpoint = location
        ? `https://localhost:7181/api/joblisting/citystate/${encodeURIComponent(location)}`
        : `https://localhost:7181/api/joblisting`;

      const response = await axios.get<JobItemProps[]>(endpoint, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {location },
      });

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

  useEffect(() => {
    fetchJobs();
  }, [getToken]);

  const handleSearch = (location: string) => {
    fetchJobs(location);
  };

  if (loading) return <h3>Loading...</h3>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <LocationSearchBar onSearch={handleSearch} />
      <div className="job-list">
        {jobs.map((job) => (
          <JobItem key={job.jobID} {...job} />
        ))}
      </div>
    </div>
  );
};

export default JobList;
