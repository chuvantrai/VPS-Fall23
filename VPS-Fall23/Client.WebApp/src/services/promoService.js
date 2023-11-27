import { useAxios } from '@/hooks';
const BASE_URI = 'api/PromoCode'
const usePromoService = () => {
    const axios = useAxios();
    const getByCode = (code, parkingZoneId) => {
        return axios.get(`${BASE_URI}/GetPromoCode/${code}`, {
            params: {
                parkingZoneId: parkingZoneId
            }
        });
    }

    return {
        getByCode
    }

}
export default usePromoService;