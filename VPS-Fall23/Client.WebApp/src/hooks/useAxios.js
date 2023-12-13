/* eslint-disable no-unused-vars */
import axios from 'axios';
import { App, notification } from 'antd';
import store from '../stores/index';
import { setGlobalState } from '../stores/systems/global.store';
import { useNavigate } from 'react-router-dom';
import { getAccountJwtModel } from '@/helpers/index.js';
import getAccountDataByCookie from '@/helpers/getAccountDataByCookie.js';
import Cookies from 'js-cookie';
import { isAfter } from 'date-fns';

const useAxios = () => {
  const app = App.useApp();
  const navigate = useNavigate();
  const errorHandler = (error) => {
    if (error === null) return;
    /***
     *
     * TODO
     * Xử lý khi lỗi 401 (token hết hạn)
     *
     */
    const account = getAccountJwtModel();
    const accountLogin = getAccountDataByCookie();
    if (error.request?.status === 401 &&
        account !== null &&
        isAfter(new Date(), account.Expires) &&
        accountLogin !== null) {
      // token hết hạn
      const _axios = axios.create({
        baseURL: import.meta.env.VITE_API_GATEWAY,
        xsrfHeaderName: 'RequestVerificationToken',
      });
      _axios
          .post('/api/Auth/AuthLogin', accountLogin)
          .then((response) => {
            Cookies.set('ACCESS_TOKEN', response.data.accessToken);
            notification.warning({
              message: 'Phiên đăng nhập hết hạn',
              description: `Token tài khoản vừa được làm mới hãy thử lại!`,
              placement: 'topRight',
            });
          })
          .catch(() => {
            app.notification.error({
              message: 'Lỗi',
              description: error.response?.data?.message,
              placement: 'topRight',
            });
            navigate('/login');
          });
      return;
    } else if (error.request?.status === 401) {
      app.notification.error({
        message: 'Lỗi',
        description: error.response?.data?.message,
        placement: 'topRight',
      });
      navigate('/login');
      return;
    }
    //Xử lý khi response trả về là arraybuffer
    if (error.request?.responseType === 'arraybuffer') {
      let errorObject = JSON.parse(new TextDecoder().decode(error?.response?.data));
      notification.error({
        message: 'Lỗi',
        description: errorObject.message,
        placement: 'topRight',
      });
    }else {
      notification.error({
        message: 'Lỗi',
        description: error?.response?.data?.message,
        placement: 'topRight',
      });
    }
  };
  const _axios = axios.create({
    baseURL: import.meta.env.VITE_API_GATEWAY,
    xsrfHeaderName: 'RequestVerificationToken',
    withCredentials: true,
  });
  _axios.interceptors.request.use((config) => {
    store.dispatch(setGlobalState({ isLoading: true }));
    return config;
  });
  _axios.interceptors.response.use(
    (response) => {
      store.dispatch(setGlobalState({ isLoading: false }));
      return Promise.resolve(response);
    },
    (error) => {
      store.dispatch(setGlobalState({ isLoading: false }));
      errorHandler(error);
      return Promise.reject(error?.response?.data);
    },
  );
  return _axios;
};

export default useAxios;
