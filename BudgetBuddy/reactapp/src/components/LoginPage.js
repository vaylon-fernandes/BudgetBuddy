import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuth } from "../services/AuthContext";

const LoginPage = (props) => {
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [captchaResponse, setCaptchaResponse] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      // TODO - Uncomment when captcha API is working.
      // const captchaResult = await axios.post("/api/captcha-verify", {
      //   captchaResponse,
      // });
      // if (!captchaResult.data.success) {
      //   setErrorMessage("Captcha verification failed");
      //   return;
      // }

      // TODO - Replace with actual login logic (Post method).
      const loggedInUser = await login({ email, password });
      
      if (loggedInUser.success===true) {
        setErrorMessage(null);
        navigate("/dashboard");
      } else {

        setErrorMessage(loggedInUser.messageList[0]);
      }
    } catch (error) {
      console.error(error);
      setErrorMessage("An error occurred");
    }
  };

  const verifyRecaptchaCallback = (response) => {
    setCaptchaResponse(response);
  };

  return (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <h1 className="display-5 fw-bold lh-1 mb-3">Login Page</h1>
          </div>
          <div className="col-md-8 col-lg-6 col-xl-4 offset-xl-1">
            <form>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="email">
                  Email address
                </label>
                <input
                  type="email"
                  id="email"
                  className="form-control"
                  placeholder="Enter your registered email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="password">
                  Password
                </label>
                <input
                  type="password"
                  id="password"
                  className="form-control"
                  placeholder="Enter your password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
              <div className="form-group">
                Captcha here {/* TODO - Captcha implementation. */}
                {/* <div
                  className="g-recaptcha"
                  data-sitekey="6LfKURIUAAAAAO50vlwWZkyK_G2ywqE52NU7YO0S"
                  data-callback={verifyRecaptchaCallback}
                  data-expired-callback="expiredRecaptchaCallback"
                ></div>
                <input
                  className="form-control d-none"
                  data-recaptcha="true"
                  required
                  data-error="Please complete the Captcha"
                />
                <div className="help-block with-errors"></div> */}
              </div>
              <button
                type="button"
                className="btn btn-primary btn-block mb-4 form-control"
                onClick={handleLogin}
              >
                Sign in
              </button>

              <div className="text-center">
                <p>
                  Not a member? <Link to="/register">Register</Link>
                </p>
              </div>
            </form>
            {errorMessage}
          </div>
        </div>
      </div>
    </>
  );
};

export default LoginPage;
