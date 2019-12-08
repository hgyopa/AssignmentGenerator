import React from "react";
import "./App.css";
import NavBar from "./components/navBar";
import { Switch, Route, Redirect } from "react-router-dom";
import Dashboard from "./components/dashboard";
import GeneratorView from "./components/generatorView";
import AssignmentView from "./components/assignmentView";

const webApiBaseUrl =
  process.env.NODE_ENV === "development"
    ? "https://localhost:44327/api/"
    : "../AssignmentGeneratorApi/api/";

function App() {
  return (
    <React.Fragment>
      <NavBar />
      <main role="main" className="container">
        <Switch>
          <Route
            path="/dashboard"
            render={props => (
              <Dashboard {...props} webApiBaseUrl={webApiBaseUrl} />
            )}
          ></Route>
          <Route
            path="/generate"
            render={props => (
              <GeneratorView {...props} webApiBaseUrl={webApiBaseUrl} />
            )}
          ></Route>
          <Route
            path="/assignment/:id"
            render={props => (
              <AssignmentView {...props} webApiBaseUrl={webApiBaseUrl} />
            )}
          ></Route>
          <Route
            path="/assignment"
            render={props => (
              <AssignmentView {...props} webApiBaseUrl={webApiBaseUrl} />
            )}
          ></Route>
          <Redirect to="/dashboard" />
        </Switch>
      </main>
    </React.Fragment>
  );
}

export default App;
