const initialState = {
    dataSets:[]
}

function reducer(state = initialState, action) {
    switch (action.type) {
        
      case "UPDATE DATASETS": {
        return {
            dataSets: action.payload.dataSets,
        };
      }
      default: {
        return state;
      }
    }
  }
  export default reducer;
  