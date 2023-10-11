import { useAxios } from '@/hooks';
import { useCallback } from 'react';


const CITY_URI = "api/City"
const COMMUNE_URI = "api/Commune"
const DISTRICT_URI = "api/District"
const useAddressServices = () => {
    const axios = useAxios();

    const getCities = () => {
        return axios.get(CITY_URI)
    }
    const getCommunes = (districtId = null) => {
        let commnueUri = COMMUNE_URI
        if (districtId) commnueUri += `/GetByDistrict/${districtId}`
        return axios.get(commnueUri)
    }
    const getDistricts = (cityId = null) => {
        let districtUri = DISTRICT_URI
        if (cityId) districtUri += `/GetByCity/${cityId}`
        return axios.get(districtUri)
    }
    return {
        getCities, getCommunes, getDistricts
    }

}
export default useAddressServices;