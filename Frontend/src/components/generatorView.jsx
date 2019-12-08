import React, { Component } from "react";
import { Form } from "react-bootstrap";
import axios from "axios";
import { toast } from "react-toastify";
import ls from "local-storage";

class GeneratorView extends Component {
  state = { title: "", text: "" };

  apiClient;

  constructor(props) {
    super(props);
    const { webApiBaseUrl } = this.props;

    this.apiClient = axios.create({
      baseURL: webApiBaseUrl,
      headers: { Authorization: `Bearer ${ls.get("token")}` }
    });
  }

  generateTest = async () => {
    await this.apiClient
      .post(`api/TestGenerator/GenerateTest`, {
        Title: this.state.title,
        Text: this.state.text
      })
      .then(response => {
        this.props.history.push({
          pathname: "/assignment",
          state: { assignment: response.data }
        });
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  handleTitleChange = event => {
    this.setState({ title: event.target.value });
  };

  handleTextChange = event => {
    this.setState({ text: event.target.value });
  };

  handleGenerate = async () => {
    await this.generateTest();
  };

  handleError = error => {
    console.log(error);
    toast("There was a problem while communicating with the Api", {
      type: toast.TYPE.ERROR
    });
  };

  render() {
    const { title, text } = this.state;

    return (
      <React.Fragment>
        <h1 className="mt-3">Generate new assignment</h1>
        <Form>
          <Form.Group controlId="formGroupEmail">
            <Form.Label>Title</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter title"
              onChange={this.handleTitleChange}
            />
          </Form.Group>
          <Form.Group controlId="formGroupPassword">
            <Form.Label>Text</Form.Label>
            <Form.Control
              type="text"
              as="textarea"
              rows="10"
              placeholder="Paste text"
              onChange={this.handleTextChange}
            />
          </Form.Group>
        </Form>
        <button
          className={"btn btn-primary" + (!(title && text) ? " disabled" : "")}
          onClick={this.handleGenerate}
        >
          Generate
        </button>
      </React.Fragment>
    );
  }
}

export default GeneratorView;
