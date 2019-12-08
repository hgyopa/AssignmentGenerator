import React, { Component } from "react";
import { Table } from "react-bootstrap";
import axios from "axios";
import ls from "local-storage";

class Dashboard extends Component {
  state = { selectedTitleId: -1, assignments: [] };

  apiClient;

  constructor(props) {
    super(props);

    const { webApiBaseUrl } = this.props;

    this.apiClient = axios.create({
      baseURL: webApiBaseUrl,
      headers: { Authorization: `Bearer ${ls.get("token")}` }
    });
  }

  async componentDidMount() {
    await this.getAssignments();
  }

  getAssignments = async () => {
    await this.apiClient
      .get("api/TestGenerator/GetAssignments")
      .then(response => {
        this.setState({
          assignments: response.data
        });
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  handleError = error => {
    console.log(error);
  };

  handleSelect = id => {
    this.setState({ selectedTitleId: id });
  };

  handleAssignmentOpen = () => {
    this.props.history.push(`/assignment/${this.state.selectedTitleId}`);
  };

  render() {
    const { selectedTitleId, assignments } = this.state;

    return (
      <React.Fragment>
        <h1 className="mt-3 mb-5">Your assignments</h1>
        <Table responsive striped bordered hover>
          <thead>
            <tr>
              <th>Title</th>
              <th>Creation date</th>
            </tr>
          </thead>
          <tbody>
            {assignments.map(assignment => (
              <tr
                key={assignment.Id}
                className={
                  assignment.Id === selectedTitleId ? "bg-primary" : ""
                }
                onClick={() => this.handleSelect(assignment.Id)}
              >
                <td>{assignment.Title}</td>
                <td>{assignment.CreationDate}</td>
              </tr>
            ))}
          </tbody>
        </Table>
        <button
          className={
            "btn btn-primary" + (selectedTitleId === -1 ? " disabled" : "")
          }
          onClick={this.handleAssignmentOpen}
        >
          Open assignment
        </button>
      </React.Fragment>
    );
  }
}

export default Dashboard;
