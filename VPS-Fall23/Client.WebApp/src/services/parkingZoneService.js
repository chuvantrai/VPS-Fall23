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
    const getImageLink = (id) => {
        return axios.get(`${BASE_URI}/GetImageLinks/${id}`)
    }
    const getFullAddress = (parkingZone) =>
        (`${parkingZone.detailAddress}, ${parkingZone.commune.name}, ${parkingZone.commune.district.name}, ${parkingZone.commune.district.city.name}`)

    return {
        getByAddress,
        getImageLink,
        getFullAddress
    }
}
export default useParkingZoneService