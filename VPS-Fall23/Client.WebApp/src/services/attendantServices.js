import { useAxios } from '@/hooks';

const BASE_URI = 'api/Attendant';

const useAttendantService = () => {
  const axios = useAxios();

  const createAccount = (input) => {
    return axios.post(`${BASE_URI}/CreateAccount`, input);
  };

  const getListAttendant = (ownerId, pageNumber, pageSize) => {
    return axios.get(`${BASE_URI}/GetListAttendant`, {
      params: {
        ownerId,
        pageNumber,
        pageSize,
      },
    });
  };

  const searchAttendantByName = (ownerId, attendantName, pageNumber, pageSize) => {
    return axios.get(`${BASE_URI}/SearchAttendantByName`, {
      params: {
        ownerId,
        attendantName,
        pageNumber,
        pageSize,
      },
    });
  };

  return {
    createAccount,
    getListAttendant,
    searchAttendantByName,
  };
};

export default useAttendantService;
