import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  accountDataJwt: null
};

const accountSlice = createSlice({
  name: 'account',
  initialState,
  reducers: {
    setAccountDataJwt(state, action) {
      state.accountDataJwt = action.payload;
    }
  },
});

export const { setAccountDataJwt } = accountSlice.actions;
export default accountSlice.reducer;