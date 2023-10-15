import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  listFounded: [],
  isShowBookingForm: false,
};

const parkingZoneSlice = createSlice({
  name: 'parkingZone',
  initialState,
  reducers: {
    setFoundedParkingZones(state, action) {
      state.listFounded = action.payload.listFounded;
    },
    setShowBookingForm(state, action) {
      state.isShowBookingForm = action.payload.isShowBookingForm;
    },
  },
});

export const { setFoundedParkingZones, setShowBookingForm } = parkingZoneSlice.actions;
export default parkingZoneSlice.reducer;
