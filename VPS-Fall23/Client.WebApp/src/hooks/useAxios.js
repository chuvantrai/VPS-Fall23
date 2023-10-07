import axios from 'axios';
import { App } from 'antd';
import { useState, useEffect } from 'react';
import store from '../stores/index';
import { setGlobalState } from '../stores/systems/global.store';

const useAxios = () => {
  const app = App.useApp();
  const errorHandler = (error) => {
    if (error === null) return;
    /***
     *
     * TODO
     * Xử lý khi lỗi 401 (token hết hạn)
     *
     */

    //Xử lý khi response trả về là arraybuffer
    if (error.request?.responseType === 'arraybuffer') {
      let errorObject = JSON.parse(new TextDecoder().decode(error?.response?.data));
      app.message.error(`${errorObject.message}`);
      return;
    }
    app.message.error(`${error?.response?.data}`);
  }
  const _axios = axios.create({
    baseURL: import.meta.env.VITE_API_GATEWAY,
    xsrfHeaderName: 'RequestVerificationToken',
  });
  _axios.interceptors.request.use((config) => {
    store.dispatch(setGlobalState({ isLoading: true }));
    return config;
  });
  _axios.interceptors.response.use(
    (response) => {
      store.dispatch(setGlobalState({ isLoading: false }));
      return response;
    },
    (error) => {
      store.dispatch(setGlobalState({ isLoading: false }));
      errorHandler(error);
    },
  );
  return _axios;
};
export default useAxios;
