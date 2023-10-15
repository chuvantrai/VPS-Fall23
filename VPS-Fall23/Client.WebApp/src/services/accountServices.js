import { useAxios } from '@/hooks/index.js';
import moment from 'moment';

const AccountServices = () => {
  const axios = useAxios();
  const getAccountProfile = (form) => {
    axios.get('/api/Auth/GetAccountProfile')
      .then((res) => {
        if (res.status === 200) {
          const profile = res.data;
          form.setFieldsValue({
            firstName: profile.firstName,
            lastName: profile.lastName,
            email: profile.email,
            phone: profile.phone,
            // commune: 2,
            address: profile.address,
            role: profile.role,
            dob: moment(profile.dob).format("DD-MM-YYYY")
            // avatar: profile.avatar,
          });
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }

  const updateAccountProfile = (req, res) => {

  }

  return { getAccountProfile };
}

export default AccountServices;