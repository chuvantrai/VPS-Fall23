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

  const getBookedOverview = async ({ parkingZoneName }) => {
    if (parkingZoneName === null || parkingZoneName === undefined || parkingZoneName === '') {
      parkingZoneName = 'all';
    }

    if (parkingZoneName !== null && parkingZoneName !== undefined && parkingZoneName !== '') {
      console.log(parkingZoneName);
      return axios.get(`${BASE_URI}/GetBookedOverview?parkingZoneName="${parkingZoneName}"`);
    }
  };

  return {
    getAllOwner,
    getOwnerByEmail,
    getBookedOverview,
  };
};

export default useParkingZoneService;
