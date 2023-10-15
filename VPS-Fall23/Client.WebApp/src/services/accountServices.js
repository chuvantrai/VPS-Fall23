import { useAxios } from '@/hooks/index.js';
import moment from 'moment';
import Cookies from 'js-cookie';
import { convertAccountDataToCode, keyNameCookies } from '@/helpers/index.js';
import { notification } from 'antd';

const AccountServices = () => {
  const axios = useAxios();
  const getAccountProfile = (form, test123) => {
    axios.get('/api/Auth/GetAccountProfile')
      .then((res) => {
        if (res.status === 200) {
          console.log(123);
          const profile = res.data;
          test123(profile.addressArray);
          form.setFieldsValue({
            firstName: profile.firstName,
            lastName: profile.lastName,
            email: profile.email,
            phone: profile.phone,
            // addressArray: profile.addressArray,
            address: profile.address,
            role: profile.role,
            dob: moment(profile.dob).format('DD-MM-YYYY'),
            // avatar: profile.avatar,
          });
        }
      })
      .catch((error) => {
        console.log(error);
      });
  };

  const updateAccountProfile = (values, communeId, fileImg) => {
    values = {
      ...values,
      phoneNumber: values.phone,
      avatarImages: fileImg,
      communeId: communeId,
    };
    axios.put('/api/Auth/UpdateProfileAccount', values)
      .then((res) => {
        if (res.status === 200) {
          Cookies.set(keyNameCookies.ACCESS_TOKEN, res.data.accessToken);
          notification.success({
            message: 'Cập nhật thành công',
          });
        }
      })
      .catch((error) => {
        notification.error({
          message: 'Lỗi',
          description: error.message,
        });
      });
  };

  return { getAccountProfile, updateAccountProfile };
};

export default AccountServices;