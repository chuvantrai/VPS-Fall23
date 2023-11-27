import { useAxios } from '@/hooks';
import dayjs from 'dayjs';
const BASE_URI = 'api/ParkingZoneAbsent';
const getAbsentsUri = `${BASE_URI}/GetAbsents`
const useParkingZoneAbsentServices = () => {
    const axios = useAxios();
    const getAbsents = (parkingzoneId, getFrom = dayjs().toJSON()) => {
        return axios.get(getAbsentsUri, {
            params: {
                parkingZoneId: parkingzoneId,
                getFrom: getFrom
            }
        })
    }
    const deleteAbsent = (absentId) => {
        return axios.delete(`${BASE_URI}/DeleteAbsent/${absentId}`);
    }
    return {
        getAbsents,
        deleteAbsent
    }

}
export default useParkingZoneAbsentServices