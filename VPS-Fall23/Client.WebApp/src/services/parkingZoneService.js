
import { notification } from 'antd';

import { useAxios } from '@/hooks';

const BASE_URI = 'api/ParkingZone';
const GET_BY_ADDRESS_URI = `${BASE_URI}/GetByAddress`;
const REGISTER = `${BASE_URI}/Register`;

const useParkingZoneService = () => {
  const axios = useAxios();

  const register = (values) => {
    axios
      .post(REGISTER, values, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((res) => {
        console.log(res);
        // notification.success({
        //   description: res,
        // });
      })
      .catch((err) => {
        notification.error({
          message: 'Có lỗi xảy ra!',
          description: err.message,
        });
      });
  };
  const getImageLink = (id) => {
    return axios.get(`${BASE_URI}/GetImageLinks/${id}`)
}
  const getFullAddress = (parkingZone) =>
    (`${parkingZone.detailAddress}, ${parkingZone.commune.name}, ${parkingZone.commune.district.name}, ${parkingZone.commune.district.city.name}`)

  const getByAddress = (id, addressType) => {
    return axios.get(GET_BY_ADDRESS_URI, {
      params: {
        id: id,
        addressType: addressType,
      },
    });
  };

  return {
    getByAddress,
    register,
    getImageLink,
    getFullAddress
  };
};

export default useParkingZoneService;

