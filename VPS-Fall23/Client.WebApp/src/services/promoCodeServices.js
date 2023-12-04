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

  const deletePromoCode = (promoCodeId) => {
    return axios.delete(`api/PromoCode/DeletePromoCode`, {
      params: {
        promoCodeId,
      },
    });
  };

  return {
    getListPromoCode,
    createNewPromoCode,
    getPromoCodeDetails,
    updatePromoCode,
    deletePromoCode,
  };
};

export default usePromoCodeServices;
