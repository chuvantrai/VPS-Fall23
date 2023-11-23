
import axios from 'axios'
const BASE_URI = "api/GoongMap"
const useGoongMapService = () => {
    const customAxios = axios.create({
        baseURL: import.meta.env.VITE_API_GATEWAY,
        xsrfHeaderName: 'RequestVerificationToken',
        withCredentials: true,
    });
    customAxios.interceptors.response.use(
        (response) => {
            return Promise.resolve(response);
        },
        (error) => {
            return Promise.reject(error?.response?.data);
        },
    );
    const placeAutoComplete = (input, sessionToken) => {
        return customAxios.get(`${BASE_URI}/AutoComplete`, {
            params: {
                input: input,
                sessionToken: sessionToken
            }
        })
    }
    const getPlaceDetail = (placeId, sessionToken) => {
        return customAxios.get(`${BASE_URI}/PlaceDetail`, {
            params: {
                placeId: placeId,
                sessionToken: sessionToken
            }
        })
    }
    const getPlaceFromLocation = ({ lat, lng }) => {
        return customAxios.get(`${BASE_URI}/GetPlaceFromLocation`, {
            params: {
                lat: lat,
                lng: lng
            }
        })
    }
    return {
        placeAutoComplete,
        getPlaceDetail,
        getPlaceFromLocation
    }

}
export default useGoongMapService;