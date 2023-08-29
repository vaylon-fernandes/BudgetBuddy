import React, { useState } from "react";
import { Link, useNavigate} from "react-router-dom";
import { useAuth } from "../services/AuthContext";
import ViewReport from "./ViewReport";
import ManageExpenses from "./ManageExpenses";

const DashboardPage = (props) => {
  const { userEmail,logout } = useAuth();
  const [activeTab, setActiveTab] = useState("home");
  const navigate = useNavigate();

  const handleTabClick = (tabId) => {
    setActiveTab(tabId);
  };

  const handleLogout = () => {
    logout();
    // Perform any additional cleanup, like redirecting to the login page.
    navigate("/login");
  };

  
    return userEmail ? (
        <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back to home"}
      </Link>
      <div className="container col-xxl-8 px-4 py-5">
        <div className="row flex-lg-row align-items-center g-5 py-5">
          <div className="col-lg-6">
            <h1 className="display-5 fw-bold lh-1 mb-3">Dashboard Page</h1>
            <p>Welcome, {userEmail}!</p>
            <button onClick={handleLogout}>Logout</button>
            <div className="row">
              <div className="col-3">
                <div
                  className="nav flex-column nav-pills"
                  role="tablist"
                  aria-orientation="vertical"
                >
                  <button
                    className={`nav-link ${activeTab === "expenses" ? "active" : ""}`}
                    onClick={() => handleTabClick("expenses")}
                  >
                    Expenses
                  </button>
                  <button
                    className={`nav-link ${activeTab === "report" ? "active" : ""}`}
                    onClick={() => handleTabClick("report")}
                  >
                    Report
                  </button>
                  <button
                    className={`nav-link ${activeTab === "charts" ? "active" : ""}`}
                    onClick={() => handleTabClick("charts")}
                  >
                    Chart
                  </button>

                  {/* Add more tabs as needed */}
                </div>
              </div>
              <div className="col-9">
                <div className="tab-content">
                  <div className={`tab-pane ${activeTab === "expenses" ? "show active" : ""}`}>
                  <ManageExpenses></ManageExpenses>
                  </div>
                  <div className={`tab-pane ${activeTab === "report" ? "show active" : ""}`}>
                  <ViewReport></ViewReport>
                  </div>
                  <div className={`tab-pane ${activeTab === "charts" ? "show active" : ""}`}>
                  Charts
                  </div>
                  {/* Add more tab content */}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>  ) : (
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

