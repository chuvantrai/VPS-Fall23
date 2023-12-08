import { combineReducers, configureStore } from "@reduxjs/toolkit";
import globalReducer from './systems/global.store'
import parkingZoneReducer from "./parkingZones/parkingZone.store";
import accountReducer from "./account/account.store.js";
const rootReducer = combineReducers({
    global: globalReducer,
    parkingZone: parkingZoneReducer,
    account: accountReducer,
});
const store = configureStore({
    reducer: rootReducer,
});


export default store;