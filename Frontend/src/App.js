import React from "react";
import "./App.css";
import NavBar from "./components/navBar";
import { Switch, Route, Redirect } from "react-router-dom";
import Dashboard from "./components/dashboard";
import GeneratorView from "./components/generatorView";
import AssignmentView from "./components/assignmentView";

function App() {
  return (
    <React.Fragment>
      <NavBar />
      <main role="main" className="container">
        <Switch>
          <Route path="/dashboard" component={Dashboard}></Route>
          <Route path="/generate" component={GeneratorView}></Route>
          <Route path="/assignment" component={AssignmentView}></Route>
          <Redirect to="/dashboard" />
        </Switch>
      </main>
    </React.Fragment>
  );
}

export default App;
