import React from "react";
import { Link } from "react-router-dom";

const RegisterPage = (props) => {
  return (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <h1 className="display-5 fw-bold lh-1 mb-3">Register Page</h1>
          </div>
        </div>
      </div>
    </>
  );
};

export default RegisterPage;
