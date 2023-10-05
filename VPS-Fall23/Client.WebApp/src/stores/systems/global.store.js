import { createSlice } from '@reduxjs/toolkit';



const initialState = {

  isLoading: true
};

const globalSlice = createSlice({
  name: 'global',
  initialState,
  reducers: {
    setGlobalState(state, action) {
      Object.assign(state, action.payload);

    }
  },
});

export const { setGlobalState } = globalSlice.actions;
export default globalSlice.reducer;
