import React from "react";
import "./App.css";
import Lmenu from "./LeftMenu/LleftMenu";
import CreateDataSet from "./CreateDataSet/CreateDataSet";
import DataSet from "./DataSet/DataSet";
import { Route, Switch } from "react-router-dom";

class App extends React.Component {
  render() {
    return (
      <div className="App">
        <Lmenu />
        <Switch>
          <Route path="/datasets/:id" component={DataSet} />
          <Route path="/" component={CreateDataSet} />
        </Switch>
      </div>
    );
  }
}

export default App;
