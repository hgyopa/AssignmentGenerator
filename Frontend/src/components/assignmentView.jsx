import React, { Component } from "react";
import _ from "lodash";

class AssignmentView extends Component {
  state = {
    assignment: {
      title: "Title",
      questions: [
        {
          id: 1,
          text: "asdafksdvjsdnmvjsd sdjnsjd cajieeandfajedajnda caw cawjd ",
          answer: "2019"
        },
        {
          id: 2,
          text: "asdafksdvjsdnmvjsd sdjnsjd cajieeandfajedajnda caw cawjd ",
          answer: "2018"
        },
        {
          id: 3,
          text: "asdafksdvjsdnmvjsd sdjnsjd cajieeandfajedajnda caw cawjd ",
          answer: "2017"
        }
      ]
    }
  };

  handleDelete = idToDelete => {
    const clone = { ...this.state.assignment };
    _.remove(clone.questions, { id: idToDelete });
    this.setState({ assignment: clone });
  };

  handleQuestionTextChange = (event, questionId) => {
    const clone = { ...this.state.assignment };
    const question = _.find(clone.questions, {
      id: questionId
    });
    question.text = event.target.value;
    this.setState({ assignment: clone });
  };

  handleQuestionAnswerChange = (event, questionId) => {
    const clone = { ...this.state.assignment };
    const question = _.find(clone.questions, {
      id: questionId
    });
    question.answer = event.target.value;
    this.setState({ assignment: clone });
  };

  render() {
    var count = 1;
    const { assignment } = this.state;

    return (
      <React.Fragment>
        <h1 className="mt-3">{assignment.title}</h1>
        {assignment.questions.map(question => (
          <div key={question.id}>
            <label>{count++}. Question</label>
            <div className="row rounded border p-4 mb-2">
              <div className="col">
                <input
                  className="form-control"
                  type="text"
                  value={question.text}
                  onChange={event =>
                    this.handleQuestionTextChange(event, question.id)
                  }
                />
              </div>
              <div className="col-md-auto">
                <div className="input-group">
                  <div className="input-group-prepend">
                    <span className="input-group-text">Answer:</span>
                  </div>
                  <input
                    type="text"
                    className="form-control"
                    value={question.answer}
                    onChange={event =>
                      this.handleQuestionAnswerChange(event, question.id)
                    }
                  />
                </div>
              </div>
              <div className="col col-lg-1">
                <button
                  className="btn btn-danger"
                  onClick={() => this.handleDelete(question.id)}
                >
                  Delete
                </button>
              </div>
            </div>
          </div>
        ))}
        <div className="row justify-content-md-center mt-4">
          <button className="btn btn-success mr-4">Save Changes</button>
          <button className="btn btn-primary ml-4 mr-4">
            Export Questions
          </button>
          <button className="btn btn-primary ml-4">Export Answers</button>
        </div>
      </React.Fragment>
    );
  }
}

export default AssignmentView;
