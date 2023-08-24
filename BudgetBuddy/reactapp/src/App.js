import React, { Component } from "react";
import LandingPage from "./components/LandingPage";

export default class App extends Component {
  static displayName = App.name;

  render() {

    return (
      <LandingPage />
    );
  }
}
