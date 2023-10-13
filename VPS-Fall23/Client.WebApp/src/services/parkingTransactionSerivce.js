import { useAxios } from '@/hooks'

const BASE_URI = "api/ParkingTransaction"
const BOOKING_URI = `${BASE_URI}/Booking`;
const useParkingTransactionService = () => {
    const axios = useAxios();

    const bookingSlot = (parkingTransaction) => {
        return axios.post(BOOKING_URI, parkingTransaction)
    }
    return {
        bookingSlot
    }


}
export default useParkingTransactionService