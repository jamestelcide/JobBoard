import React from "react";
import JobItem from "./JobItem";
import "../css/JobList.css";

const JobList: React.FC = () => {
  const jobs = [
    {
      JobID: "BFBC1319-B732-4D1C-A23B-AFB473014F1F",
      JobTitle: "Software Engineer",
      CompanyName: "Tech Innovations",
      Email: "hr@techinnovations.com",
      CityAndState: "Atlanta, GA",
      PayRange: "$80,000 - $120,000",
      jobType: "FullTime",
      JobPostedDate: new Date("2024-10-20T00:00:00Z"),
      FullDescription:
        "Seeking a software engineer with experience in web and mobile application development. Must be proficient in C# and JavaScript.",
    },
    {
      JobID: "F358F000-46D2-4D56-95BC-05375645A6FC",
      JobTitle: "Data Scientist",
      CompanyName: "Tech Innovations",
      Email: "hr@techinnovations.com",
      CityAndState: "New York, NY",
      PayRange: "$100,000 - $130,000",
      jobType: "FullTime",
      JobPostedDate: new Date("2024-10-20T00:00:00Z"),
      FullDescription:
        "Seeking a software engineer with experience in web and mobile application development. Must be proficient in C# and JavaScript.",
    },
  ];

  return (
    <div className="job-list">
      {jobs.map((job, index) => (
        <JobItem key={index} {...job} />
      ))}
    </div>
  );
};

export default JobList;
