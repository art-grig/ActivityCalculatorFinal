import React from "react";
import "./LeftMenu.css";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import store from "../redux/store";

class LMenu extends React.Component {
  state = {
    showList: false,
    dataSets: [],
  };

  componentDidMount() {
    store.subscribe(() => {
      const state = store.getState();
      this.setState({ dataSets: state.dataSets });
    });
    const getDataSets = async () => {
      await fetch("http://localhost:5000/api/v1/Datasets")
        .then((response) => response.json())
        .then((data) => {
          console.log(data);
          this.setState({
            dataSets: [...data.data],
          });
        });
    };

    getDataSets();
  }

  render() {
    console.log(this.state);
    return (
      <div className="left-menu">
        <div
          onClick={() => this.setState({ showList: !this.state.showList })}
          className="left-menu__apps"
        >
          <svg
            className={
              this.state.showList
                ? "left-menu__arrow-svg svg-rotate"
                : "left-menu__arrow-svg"
            }
            width="18"
            height="10"
            viewBox="0 0 18 10"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path d="M1 1L9 9L17 1" stroke="#5D6D97" strokeLinecap="round" />
          </svg>
          <p>Datasets</p>
        </div>
        <div className="left-menu__datasetsList">
          {this.state.showList &&
            this.state.dataSets.map((dataSet) => {
              return (
                <Link
                  to={"/datasets/" + dataSet.id}
                  key={dataSet.id}
                  className="left-menu__appLink"
                >
                  {dataSet.name}
                </Link>
              );
            })}
        </div>
        <div className="left-menu__svg-line"></div>
        <Link to="/" className="left-menu__add-app-link">
          <div className="left-menu__apps">
            <svg
              className="left-menu__arrow-svg"
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path d="M9 1V17" stroke="#5D6D97" strokeLinecap="round" />
              <path d="M17 9L1 9" stroke="#5D6D97" strokeLinecap="round" />
            </svg>
            <p>Add Dataset</p>
          </div>
        </Link>
        <div className="left-menu__svg-line"></div>
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
const updateDataSets = functionFromConnect(LMenu);

export default updateDataSets;
