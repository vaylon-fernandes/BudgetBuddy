import React from "react";
import logo from "../assets/Hero_1.png";
import { Link } from "react-router-dom";

const LandingPage = (props) => {
  return (
    <div className="container col-xxl-8 px-4 py-5">
      <div className="row flex-lg-row-reverse align-items-center g-5 py-5">
        <div className="col-10 col-sm-8 col-lg-6">
          <img
            src={logo}
            className="d-block mx-lg-auto img-fluid"
            alt="Bootstrap Themes"
            width="700"
            height="500"
            loading="lazy"
          />
        </div>
        <div className="col-lg-6">
          <h1 className="display-5 fw-bold lh-1 mb-3">Budget Buddy</h1>
          <p className="lead">Hello.</p>
          <div className="d-grid gap-2 d-md-flex justify-content-md-start">
            <Link to="/login" className="btn btn-primary btn-lg px-4">
              Sign in
            </Link>
            <Link
              to="/register"
              className="btn btn-outline-primary btn-lg px-4"
            >
              sign up
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LandingPage;
