import { Link, useRouteError } from "react-router-dom";

export default function ErrorPage() {
  const error = useRouteError();
  console.error(error);

  return (
    <>
      <Link to="/" className="px-4 text-decoration-none">
        {"<<  Back"}
      </Link>
      <div id="error-page">
        <div className="container col-xxl-8 px-4 py-5">
          <div className="row flex-lg-row align-items-center g-5 py-5">
            <div className="col-lg-6">
              <h1>Oops!</h1>
              <p>Sorry, an unexpected error has occurred.</p>
              <p>
                <i>{error.statusText || error.message}</i>
              </p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
