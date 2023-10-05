import { combineReducers, configureStore } from "@reduxjs/toolkit";
import globalReducer from './systems/global.store'
const rootReducer = combineReducers({
    global: globalReducer,
});
const store = configureStore({
    reducer: rootReducer,
});


export default store;