import { useAxios } from '@/hooks/index.js';
import moment from 'moment';
import Cookies from 'js-cookie';
import { keyNameCookies } from '@/helpers/index.js';
import { notification } from 'antd';


const AccountServices = () => {
  const axios = useAxios();
  const getAccountProfile = (form, test123) => {
    axios.get('/api/Auth/GetAccountProfile')
      .then((res) => {
        if (res.status === 200) {
          const profile = res.data;
          test123(profile.addressArray, profile.commune);
          form.setFieldsValue({
            firstName: profile.firstName,
            lastName: profile.lastName,
            email: profile.email,
            phone: profile.phone,
            // addressArray: profile.addressArray,
            address: profile.address,
            role: profile.role,
            dob: moment(profile.dob).format('DD-MM-YYYY'),
            roleId: profile.roleId,
            avatar: profile.avatar
          });
        }
      })
      .catch((error) => {
        console.log(error);
      });
  };

  const updateAccountProfile = (values, communeId, fileImg) => {
    const formData = new FormData();
    formData.append('firstName', values.firstName);
    formData.append('lastName', values.lastName);
    formData.append('phoneNumber', values.phone);
    formData.append('address', values.address);
    if (communeId !== undefined) formData.append('communeId', communeId);
    formData.append('avatarImages', fileImg);
    axios.put('/api/Auth/UpdateProfileAccount', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    })
      .then((res) => {
        if (res.status === 200) {
          Cookies.set(keyNameCookies.ACCESS_TOKEN, res.data.accessToken);
          notification.success({
            message: 'Cập nhật thành công',
          });
          window.location.reload();
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