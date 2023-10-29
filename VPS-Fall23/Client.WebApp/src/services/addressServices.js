import { useAxios } from '@/hooks';
import { useCallback } from 'react';


const CITY_URI = "api/City"
const COMMUNE_URI = "api/Commune"
const DISTRICT_URI = "api/District"
const useAddressServices = () => {
    const axios = useAxios();

    const getCities = (id = null) => {
        let uri = CITY_URI;
        if (id) {
            uri += "/" + id
        }
        return axios.get(uri)
    }
    const getDistrictById = (id) => {
        let uri = DISTRICT_URI + "/" + id;
        return axios.get(uri)
    }
    const getCommuneById = (id) => {
        let uri = COMMUNE_URI + "/" + id;
        return axios.get(uri)
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
        getCities, getCommunes, getDistricts, getDistrictById, getCommuneById
    }

}
export default useAddressServices;