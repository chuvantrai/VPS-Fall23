import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  isShowBookingForm: false,
  listDataBookmark: [],
  selectedLocation: null
};

const parkingZoneSlice = createSlice({
  name: 'parkingZone',
  initialState,
  reducers: {
    setShowBookingForm(state, action) {
      state.isShowBookingForm = action.payload.isShowBookingForm;
    },
    setListDataBookmark(state, action) {
      state.listDataBookmark = action.payload.listDataBookmark;
    },
    setSelectedLocation(state, action) {
      state.selectedLocation = action.payload.selectedLocation
    }
  },
});

export const { setShowBookingForm, setListDataBookmark, setSelectedLocation } = parkingZoneSlice.actions;
export default parkingZoneSlice.reducer;
