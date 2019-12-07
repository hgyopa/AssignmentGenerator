import React from "react";
import { NavLink } from "react-router-dom";

const NavBar = () => {
  return (
    <nav className="navbar navbar-expand-md navbar-dark bg-dark mb-4">
      <NavLink className="navbar-brand" to="/dashboard">
        Assignment Generator
      </NavLink>
      <div className="collapse navbar-collapse">
        <ul className="navbar-nav mr-auto">
          <li className="nav-item">
            <NavLink className="nav-link" to="/dashboard">
              Dashboard
            </NavLink>
          </li>
          <li className="nav-item">
            <NavLink className="nav-link" to="/generate">
              Generate
            </NavLink>
          </li>
          <li className="nav-item">
            <NavLink className="nav-link" to="/assignment">
              Assignment
            </NavLink>
          </li>
        </ul>
        <ul className="navbar-nav mt-2 mt-md-0">
          <li className="nav-item">
            <NavLink className="nav-link" to="/logout">
              Log out
            </NavLink>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default NavBar;
