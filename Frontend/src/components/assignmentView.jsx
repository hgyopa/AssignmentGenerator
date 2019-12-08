import React, { Component } from "react";
import _ from "lodash";
import axios from "axios";
import { toast } from "react-toastify";
import ls from "local-storage";

class AssignmentView extends Component {
  state = {
    assignment: null
  };

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
    const { match, location } = this.props;

    if (match.params.id) {
      await this.getAssignment();
    } else {
      var id = 1;
      location.state.assignment.Questions.forEach(element => {
        element.Id = id++;
      });

      this.setState({
        assignment: location.state.assignment
      });
    }
  }

  getAssignment = async () => {
    await this.apiClient
      .get(`api/TestGenerator/GetAssignment/${this.props.match.params.id}`)
      .then(response => {
        this.setState({
          assignment: response.data
        });
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  createAssignment = async () => {
    await this.apiClient
      .post(`api/TestGenerator/CreateAssignment`, this.state.assignment)
      .then(response => {
        this.setState({
          assignment: response.data
        });
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  updateAssignment = async () => {
    await this.apiClient
      .put(
        `api/TestGenerator/UpdateAssignment/${this.state.assignment.Id}`,
        this.state.assignment
      )
      .then(response => {
        this.setState({
          assignment: response.data
        });
      })
      .catch(error => {
        this.handleError(error);
      });
  };

  handleError = error => {
    console.log(error);
    toast("There was a problem while communicating with the Api", {
      type: toast.TYPE.ERROR
    });
  };

  handleDelete = idToDelete => {
    const clone = { ...this.state.assignment };
    _.remove(clone.Questions, { Id: idToDelete });
    this.setState({ assignment: clone });
  };

  handleQuestionTextChange = (event, questionId) => {
    const clone = { ...this.state.assignment };
    const question = _.find(clone.Questions, {
      Id: questionId
    });
    question.Text = event.target.value;
    this.setState({ assignment: clone });
  };

  handleQuestionAnswerChange = (event, questionId) => {
    const clone = { ...this.state.assignment };
    const question = _.find(clone.Questions, {
      Id: questionId
    });
    question.Answers[0].Text = event.target.value;
    this.setState({ assignment: clone });
  };

  handleSave = async () => {
    if (this.state.assignment.Id === 0) {
      await this.createAssignment();
    } else {
      await this.updateAssignment();
    }

    toast("Assignment saved successfully", {
      type: toast.TYPE.SUCCESS
    });
  };

  renderAssignment = assignment => {
    if (!assignment) {
      return null;
    }

    var count = 1;

    return (
      <React.Fragment>
        <h1 className="mt-3">{assignment.Title}</h1>
        {assignment.Questions.map(question => (
          <div key={question.Id}>
            <label>{count++}. Question</label>
            <div className="row rounded border p-4 mb-2">
              <div className="col">
                <input
                  className="form-control"
                  type="text"
                  value={question.Text}
                  onChange={event =>
                    this.handleQuestionTextChange(event, question.Id)
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
                    value={question.Answers[0].Text}
                    onChange={event =>
                      this.handleQuestionAnswerChange(event, question.Id)
                    }
                  />
                </div>
              </div>
              <div className="col col-lg-1">
                <button
                  className="btn btn-danger"
                  onClick={() => this.handleDelete(question.Id)}
                >
                  Delete
                </button>
              </div>
            </div>
          </div>
        ))}
        <div className="row justify-content-md-center mt-4">
          <button className="btn btn-success mr-4" onClick={this.handleSave}>
            Save Changes
          </button>
          <button className="btn btn-primary ml-4 mr-4">
            Export Questions
          </button>
          <button className="btn btn-primary ml-4">Export Answers</button>
        </div>
      </React.Fragment>
    );
  };

  render() {
    const { assignment } = this.state;

    return this.renderAssignment(assignment);
  }
}

export default AssignmentView;
