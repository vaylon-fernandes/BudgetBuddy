import React from "react";
import { Link, useLocation } from "react-router-dom";

const DashboardPage = (props) => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const email = queryParams.get("email");

  return (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <h1 className="display-5 fw-bold lh-1 mb-3">Dashboard Page</h1>
            <p>Welcome, {email}!</p>
          </div>
        </div>
      </div>
    </>
  );
};

export default DashboardPage;
