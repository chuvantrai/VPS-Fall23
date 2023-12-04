import { useAxios } from '@/hooks/index.js';

const usePromoCodeServices = () => {
  const axios = useAxios();

  const getListPromoCode = (ownerId, pageNumber, pageSize) => {
    return axios.get(`api/PromoCode/GetListPromoCode`, {
      params: {
        ownerId,
        pageNumber,
        pageSize,
      },
    });
  };

  const createNewPromoCode = (input) => {
    return axios.post(`api/PromoCode/CreateNewPromoCode`, input);
  };

  const getPromoCodeDetails = (promoCodeId) => {
    return axios.get(`api/PromoCode/GetPromoCodeDetail`, {
      params: {
        promoCodeId,
      },
    });
  };

  const updatePromoCode = (input) => {
    return axios.put(`api/PromoCode/UpdatePromoCode`, input);
  };

  return {
    getListPromoCode,
    createNewPromoCode,
    getPromoCodeDetails,
    updatePromoCode,
  };
};

export default usePromoCodeServices;
