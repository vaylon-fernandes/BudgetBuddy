import React from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../services/AuthContext";

const DashboardPage = (props) => {
  const { user } = useAuth();

  return user ? (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <h1 className="display-5 fw-bold lh-1 mb-3">Dashboard Page</h1>
            <p>Welcome, {user}!</p>
          </div>
        </div>
      </div>
    </>
  ) : (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <p>
              You are not logged in. Please <Link to="/login">login</Link>.
            </p>
          </div>
        </div>
      </div>
    </>
  );
};

export default DashboardPage;
