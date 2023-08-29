import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from 'axios';

const SignUpForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const nav=useNavigate();

  const handleSignUp = async (userData) => {
    userData.preventDefault();
    // TODO: Handle signup logic here
    try{
      const response = await axios.post(
        "http://localhost:5096/api/Auth/register",
        {
          email:email,
          password:password
        }
      );

      const signURresponse = response.data;
      if (signURresponse.success===true) {
        setErrorMessage("");
        nav("/login")
      }else{
        setErrorMessage(signURresponse.messageList[0]);
      }
    } catch (error) {
      console.error("Login error:", error);
      setErrorMessage("An error occurred");
      throw error;
    }

  };

  return (
    <>
    <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>

    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <h2 className="mb-4">Sign Up</h2>
          <form onSubmit={handleSignUp}>
            <div className="mb-3">
              <label htmlFor="email" className="form-label">
                Email address
              </label>
              <input
                type="email"
                className="form-control"
                id="email"
                placeholder="Enter your email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="password" className="form-label">
                Password
              </label>
              <input
                type="password"
                className="form-control"
                id="password"
                placeholder="Enter your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="confirmPassword" className="form-label">
                Confirm Password
              </label>
              <input
                type="password"
                className="form-control"
                id="confirmPassword"
                placeholder="Confirm your password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
              />
            </div>
            <button type="submit" className="btn btn-primary">
              Sign Up
            </button>
          </form>
        </div>
      </div>
    </div>
    <div className="text-danger">{errorMessage}</div>
    </>
  );
};

export default SignUpForm;
