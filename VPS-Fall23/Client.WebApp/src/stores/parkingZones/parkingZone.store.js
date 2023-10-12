import { createSlice } from '@reduxjs/toolkit';



const initialState = {
    listFounded: []
};

const parkingZoneSlice = createSlice({
    name: 'parkingZone',
    initialState,
    reducers: {
        setFoundedParkingZones(state, action) {
            Object.assign(state, action.payload);
        }
    },
});

export const { setFoundedParkingZones } = parkingZoneSlice.actions;
export default parkingZoneSlice.reducer;
