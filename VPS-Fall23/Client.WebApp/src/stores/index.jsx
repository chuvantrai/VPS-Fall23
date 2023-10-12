import { combineReducers, configureStore } from "@reduxjs/toolkit";
import globalReducer from './systems/global.store'
import parkingZoneReducer from "./parkingZones/parkingZone.store";
const rootReducer = combineReducers({
    global: globalReducer,
    parkingZone: parkingZoneReducer
});
const store = configureStore({
    reducer: rootReducer,
});


export default store;