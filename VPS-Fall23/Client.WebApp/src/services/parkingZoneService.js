import { useAxios } from '@/hooks';

const BASE_URI = "api/ParkingZone";
const GET_BY_ADDRESS_URI = `${BASE_URI}/GetByAddress`
const useParkingZoneService = () => {
    const axios = useAxios();


    const getByAddress = (id, addressType) => {
        return axios.get(GET_BY_ADDRESS_URI, {
            params: {
                id: id,
                addressType: addressType
            }
        })
    }
    return {
        getByAddress
    }
}
export default useParkingZoneService