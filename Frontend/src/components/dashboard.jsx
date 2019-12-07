import React, { Component } from "react";
import { Table } from "react-bootstrap";

class Dashboard extends Component {
  state = { selectedTitleId: -1 };

  assignments = [
    {
      id: 1,
      title: "asdafksdvjsdnmvjsd sdjnsjd cajieeandfajedajnda caw cawjd ",
      date: "2019-01-01"
    },
    { id: 2, title: "hfg", date: "2019-01-01" },
    { id: 3, title: "hfg", date: "2019-01-01" },
    { id: 4, title: "hfg", date: "2019-01-01" },
    { id: 5, title: "hfg", date: "2019-01-01" },
    { id: 6, title: "hfg", date: "2019-01-01" },
    { id: 7, title: "hfg", date: "2019-01-01" },
    { id: 8, title: "hfg", date: "2019-01-01" }
  ];

  handleSelect = id => {
    this.setState({ selectedTitleId: id });
  };

  handleAssignmentOpen = () => {};

  render() {
    const { selectedTitleId } = this.state;

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
            {this.assignments.map(assignment => (
              <tr
                key={assignment.id}
                className={
                  assignment.id === selectedTitleId ? "bg-primary" : ""
                }
                onClick={() => this.handleSelect(assignment.id)}
              >
                <td>{assignment.title}</td>
                <td>{assignment.date}</td>
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
