import { useAxios } from '@/hooks';

const BASE_URI = 'api/Attendant';

const useAttendantService = () => {
  const axios = useAxios();

  const createAccount = (input) => {
    return axios.post(`${BASE_URI}/CreateAccount`, input);
  };

  return {
    createAccount,
  };
};

export default useAttendantService;
