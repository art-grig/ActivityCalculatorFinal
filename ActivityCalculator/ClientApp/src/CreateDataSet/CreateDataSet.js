import React from "react";
import "./CreateDataSet.css";
import { connect } from "react-redux";
import store from '../redux/store';

const mailValidationRegExp = /^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)[0-9]{2}$/g;

class Main extends React.Component {
  state = {
    DatasetName: "",
    Description: "",
    userId: "",
    registrationDate: "",
    lastActivDate: "",
    idValid: false,
    registerValid: false,
    lastActivValid: false,
    newActivityLogs: [],
    deleteIds: [],
  };

  

  dataHandler = (e) => {
    const name = e.target.name;
    const value = e.target.value;
    this.setState({ [name]: value }, () => {
      this.validateField(name, value);
    });
  };

  toDate = (dateStr) => {
    let from = dateStr.split(".");
    return new Date(from[2], from[1] - 1, from[0]);
  };

  validateField = (type, value) => {
    let isValid;
    switch (type) {
      case "userId":
        isValid = value >= 0 && value !== "";
        this.setState({
          idValid: isValid,
        });
        break;
      case "registrationDate":
        isValid = mailValidationRegExp.test(this.state.registrationDate);
        this.setState({
          registerValid: isValid,
        });
        break;
      case "lastActivDate":
        isValid = mailValidationRegExp.test(this.state.lastActivDate);
        this.setState({
          lastActivValid: isValid,
        });
        break;
      default:
        break;
    }
  };

  addDataSetHandler = () => {
    if (
      this.state.idValid &&
      this.state.registerValid &&
      this.state.lastActivValid
    ) {
      const data = this.state.newActivityLogs
        .find((field) => field.userId == this.state.userId);
      if (data) {
        alert(`Item with that id: ${data.userId} already exists`);
        return;
      }
      if (
        this.toDate(this.state.lastActivDate) <
        this.toDate(this.state.registrationDate)
      ) {
        alert("LastActiveDate cannot be less than RegistrationDate");
        return;
      }
      this.setState({
        newActivityLogs: [
          {
            userId: +this.state.userId,
            registrationDate: this.state.registrationDate,
            lastActivityDate: this.state.lastActivDate,
          },
          ...this.state.newActivityLogs,
        ],
      });
    }
  };

  removeHandler = (data) => {
    if (!data.id) {
      const newDataSet = this.state.newActivityLogs.filter(
        (field) => data.userId !== field.userId
      );
      this.setState({
        newActivityLogs: newDataSet,
      });
    } else {
    }
  };

  saveHandler = () => {
    if (this.state.DatasetName) {
      const data = {
        name: this.state.DatasetName,
        description: this.state.Description,
        activityLogs: this.state.newActivityLogs,
        deletedIds: this.state.deleteIds,
      };
      const createDataSet = async () => {
        await fetch("http://localhost:5000/api/v1/Datasets", {
          method: "POST",
          headers: {
            "Content-type": "application/json",
          },
          body: JSON.stringify(data),
        })
          .then((response) => response.json())
          .then((data) => {            
            this.setState(
              {
                newActivityLogs: [],
                deleteIds: [],
              },
              () => this.props.history.push(`/datasets/${data.data.id}`)
            );
          });
          fetch("http://localhost:5000/api/v1/Datasets")
          .then((response) => response.json())
          .then((data) =>{
            store.dispatch({
              type:"UPDATE DATASETS",
              payload:{
                dataSets:data.data,
              }
            });
          })
      };
      createDataSet();
      
    }
  };

  render() {
    return (
      <div className="main">
        <h3 className="main__title">New dataset</h3>
        <div className="main__input-block">
          <p>Dataset Name</p>
          <input
            value={this.state.DatasetName}
            onChange={(e) => this.dataHandler(e)}
            name="DatasetName"
            className="main__input"
            type="text"
          />
        </div>
        <div className="main__input-block">
          <p className="main__desc">Description</p>
          <textarea
            value={this.state.Description}
            onChange={(e) => this.dataHandler(e)}
            name="Description"
            cols="45"
            className="main__textarea"
          />
        </div>
        <div className="main__form-titles">
          <span>UserId</span>
          <span>Date Registration</span>
          <span>Date Last Activity</span>
        </div>
        <div className="main__form">
          <input
            value={this.state.userId}
            onChange={(e) => this.dataHandler(e)}
            name="userId"
            type="number"
            className={
              this.state.idValid
                ? "main__input-form validOk"
                : "main__input-form validBad"
            }
            placeholder="1"
          />
          <input
            value={this.state.registrationDate}
            name="registrationDate"
            onChange={(e) => this.dataHandler(e)}
            type="text"
            className={
              this.state.registerValid
                ? "main__input-form validOk"
                : "main__input-form validBad"
            }
            placeholder="01.10.2015"
          />
          <input
            value={this.state.lastActivDate}
            name="lastActivDate"
            onChange={(e) => this.dataHandler(e)}
            type="text"
            className={
              this.state.lastActivValid
                ? "main__input-form validOk"
                : "main__input-form validBad"
            }
            placeholder="01.01.2016"
          />
          <div
            className="main-form__add-block"
            onClick={() => this.addDataSetHandler()}
          >
            <svg
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path d="M9 1V17" stroke="#5D6D97" strokeLinecap="round" />
              <path d="M17 9L1 9" stroke="#5D6D97" strokeLinecap="round" />
            </svg>{" "}
            Add
          </div>
        </div>
        {this.state.newActivityLogs.length > 0 ? (
          <>
            <table class="main__table">
              <thead>
                <tr>
                  <th>UserId</th>
                  <th>Date Registration</th>
                  <th>Date Last Activity</th>
                  <th>Remove</th>
                </tr>
              </thead>
              <tbody className="main__tbody">
                {this.state.newActivityLogs.map((data) => {
                  return (
                    <tr>
                      <td>{data.userId}</td>
                      <td>{data.registrationDate}</td>
                      <td>{data.lastActivityDate}</td>
                      <td
                        className="main__table-remove"
                        onClick={() => this.removeHandler(data)}
                      >
                        <svg
                          width="13"
                          height="16"
                          viewBox="0 0 132 159"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M53.3775 0.0832089C49.9635 0.0716305 46.4408 1.28302 43.9602 3.77418C41.4861 6.25875 40.2641 9.81219 40.2524 13.2639L40.23 19.8765L0.989361 19.7434L0.944508 32.9685L7.48461 32.9907L7.12578 138.791C7.08889 149.669 15.9203 158.659 26.6788 158.696L105.16 158.962C115.919 158.998 124.811 150.068 124.848 139.191L125.206 33.3899L131.747 33.4121L131.791 20.187L92.5508 20.0539L92.5732 13.4414C92.5849 9.98964 91.3871 6.42799 88.9234 3.92006C86.4662 1.42538 82.9583 0.183533 79.5379 0.171933L53.3775 0.0832089ZM53.3326 13.3083L79.493 13.397L79.4706 20.0096L53.3102 19.9209L53.3326 13.3083ZM20.5648 33.035L112.126 33.3456L111.767 139.146C111.755 142.816 108.835 145.749 105.205 145.737L26.7237 145.471C23.0939 145.458 20.1935 142.506 20.206 138.836L20.5648 33.035ZM33.5777 52.9171L33.331 125.655L46.4113 125.699L46.6579 52.9614L33.5777 52.9171ZM59.7381 53.0058L59.4915 125.744L72.5717 125.788L72.8184 53.0501L59.7381 53.0058ZM85.8986 53.0945L85.6519 125.833L98.7321 125.877L98.9788 53.1389L85.8986 53.0945Z"
                            fill="#5D6D97"
                          />
                        </svg>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
            <div className="main__buttons-block">
              <button onClick={() => this.saveHandler()} disabled={false}>
                Save
              </button>
            </div>
          </>
        ) : (
          <></>
        )}
      </div>
    );
  }
}


const mapStateToProps = (state) => {
  return {
    dataSets:state.dataSets
  };
};

const functionFromConnect = connect(mapStateToProps);
const updateDataSets = functionFromConnect(Main);

export default updateDataSets;