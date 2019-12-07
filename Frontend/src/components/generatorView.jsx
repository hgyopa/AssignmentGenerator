import React, { Component } from "react";
import { Form } from "react-bootstrap";

class GeneratorView extends Component {
  state = { title: "", text: "" };

  handleTitleChange = event => {
    this.setState({ title: event.target.value });
  };

  handleTextChange = event => {
    this.setState({ text: event.target.value });
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
          onClick={this.handleAssignmentOpen}
        >
          Generate
        </button>
      </React.Fragment>
    );
  }
}

export default GeneratorView;
