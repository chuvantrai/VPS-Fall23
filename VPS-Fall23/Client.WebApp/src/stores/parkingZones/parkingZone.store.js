import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  listFounded: [],
  isShowBookingForm: false,
  listDataBookmark: []
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
    setListDataBookmark(state, action) {
      state.listDataBookmark = action.payload.listDataBookmark;
    },
  },
});

export const { setFoundedParkingZones, setShowBookingForm, setListDataBookmark } = parkingZoneSlice.actions;
export default parkingZoneSlice.reducer;
