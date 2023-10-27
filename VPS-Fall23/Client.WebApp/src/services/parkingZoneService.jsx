import { notification } from 'antd';
import { useNavigate } from 'react-router-dom';

import { useAxios } from '@/hooks';

const BASE_URI = 'api/ParkingZone';
const GET_BY_ADDRESS_URI = `${BASE_URI}/GetByAddress`;
const REGISTER = `${BASE_URI}/Register`;

const useParkingZoneService = () => {
  const axios = useAxios();
  const navigate = useNavigate();

  const getAllParkingZone = ({ pageNumber, pageSize }) => {
    return axios.get(`${BASE_URI}/GetAll`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
      },
    });
  };

  const getParkingZoneByName = ({ pageNumber, pageSize, name }) => {
    return axios.get(`${BASE_URI}/GetByName`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
        name: name,
      },
    });
  };

  // const getParkingZoneByOwner = ({ pageNumber, pageSize, name }) => {
  //   return axios.get(`${BASE_URI}/GetByOwner`, {
  //     params: {
  //       pageNumber: pageNumber,
  //       pageSize: pageSize,
  //       name: name,
  //     },
  //   });
  // };

  const register = (values) => {
    axios
      .post(REGISTER, values, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((res) => {
        notification.success({
          message: res?.data,
        });
        setTimeout(() => {
          navigate('/');
        }, 1000);
      })
      .catch((err) => {
        notification.error({
          message: 'Có lỗi xảy ra!',
          description: err.message,
        });
      });
  };

  const getImageLink = (id) => {
    return axios.get(`${BASE_URI}/GetImageLinks/${id}`);
  };

  const getFullAddress = (parkingZone) =>
    `${parkingZone.detailAddress}, ${parkingZone.commune.name}, ${parkingZone.commune.district.name}, ${parkingZone.commune.district.city.name}`;

  const getByAddress = (id, addressType) => {
    return axios.get(GET_BY_ADDRESS_URI, {
      params: {
        id: id,
        addressType: addressType,
      },
    });
  };

  const getRequestParkingZones = ({ pageNumber, pageSize }) => {
    return axios.get(`${BASE_URI}/GetRequestedParkingZones`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
      },
    });
  };

  const changeParkingZoneStat = (params) => {
    return axios.put(`${BASE_URI}/ChangeParkingZoneStat`, params);
  };

  return {
    getByAddress,
    register,
    getImageLink,
    getFullAddress,
    getRequestParkingZones,
    changeParkingZoneStat,
    getAllParkingZone,
    getParkingZoneByName,
  };
};

export default useParkingZoneService;
