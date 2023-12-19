import { useAxios } from '@/hooks';

const BASE_URI = 'api/ParkingZoneOwner';

const useParkingZoneService = () => {
  const axios = useAxios();

  const getAllOwner = ({ pageNumber, pageSize }) => {
    return axios.get(`${BASE_URI}/GetAll`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
      },
    });
  };

  const getOwnerByEmail = ({ pageNumber, pageSize, email }) => {
    return axios.get(`${BASE_URI}/GetByEmail`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
        email: email,
      },
    });
  };

  const getBookedOverview = async ({ parkingZoneId }) => {
    console.log(parkingZoneId);
    return axios.get(`${BASE_URI}/GetBookedOverview`, {
      params: {
        parkingZoneId: parkingZoneId,
      },
    });
  };

  return {
    getAllOwner,
    getOwnerByEmail,
    getBookedOverview,
  };
};

export default useParkingZoneService;
