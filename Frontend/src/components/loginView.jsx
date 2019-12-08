import React, { Component } from "react";
import Logo from "../logo.png";
import axios from "axios";
import qs from "qs";
import ls from "local-storage";

class LoginView extends Component {
  state = { username: "", password: "" };

  apiClient;

  constructor(props) {
    super(props);
    const { webApiBaseUrl } = this.props;

    this.apiClient = axios.create({
      baseURL: webApiBaseUrl
    });
  }

  postLogin = async () => {
    await axios({
      method: "post",
      url: `${this.props.webApiBaseUrl}Token`,
      data: qs.stringify({
        username: this.state.username,
        password: this.state.password,
        grant_type: "password"
      }),
      headers: {
        "content-type": "application/x-www-form-urlencoded;charset=utf-8"
      }
    })
      .then(response => {
        ls.set("token", response.data.access_token);
        this.props.history.push("/dashboard");
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  handleLogin = async () => {
    await this.postLogin();
  };

  handleUsernameChange = event => {
    this.setState({ username: event.target.value });
  };

  handlePasswordChange = event => {
    this.setState({ password: event.target.value });
  };

  handleError = error => {
    console.log(error);
  };

  render() {
    return (
      <React.Fragment>
        <div className="form-signin">
          <img
            className="mb-4 center"
            src={Logo}
            alt=""
            width="100"
            height="100"
          />
          <h1 className="h3 mb-3 font-weight-normal">Please sign in</h1>
          <label htmlFor="inputEmail" className="sr-only">
            Email address
          </label>
          <input
            type="email"
            id="inputEmail"
            className="form-control"
            placeholder="Email address"
            required
            onChange={this.handleUsernameChange}
          />
          <label htmlFor="inputPassword" className="sr-only">
            Password
          </label>
          <input
            type="password"
            id="inputPassword"
            className="form-control"
            placeholder="Password"
            required
            onChange={this.handlePasswordChange}
          />
          <button
            className="btn btn-lg btn-primary btn-block"
            onClick={this.handleLogin}
          >
            Sign in
          </button>
        </div>
      </React.Fragment>
    );
  }
}

export default LoginView;
