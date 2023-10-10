/* eslint-disable no-unused-vars */
import { notification } from 'antd';

import { useAxios } from '@/hooks';

const useAuthService = () => {
  const axios = useAxios();

  const register = (values, callBack) => {
    axios
      .post('/api/Auth/Register', values)
      .then((res) => {
        if (res.status === 200) callBack(values);
      })
      .catch((err) => {
        notification.error({
          message: 'Có lỗi xảy ra',
          description: err,
        });
      });
  };

  const verifyAccount = (values, callBack) => {
    axios.post('api/Auth/VerifyNewAccount', values).then((res) => {
      if (res.status === 200) {
        callBack(true);
      }
    });
  };

  const resendCode = (email) => {
    axios
      .put('/api/Auth/ResendVerificationCode', email)
      .then((res) => {
        if (res.status === 200) {
          notification.success({
            message: 'Kiểm tra email của bạn để lấy mã xác thực!',
          });
        } else {
          notification.error({
            message: 'Có lỗi xảy ra',
            description: res,
          });
        }
      })
      .catch((err) => {
        notification.error({
          message: 'Có lỗi xảy ra',
          description: err,
        });
      });
  };

  return { register, verifyAccount, resendCode };
};

export default useAuthService;
