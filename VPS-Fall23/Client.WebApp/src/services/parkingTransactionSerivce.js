import { useAxios } from '@/hooks'

const BASE_URI = "api/ParkingTransaction"
const BOOKING_URI = `${BASE_URI}/Booking`;
const GET_PAY_URL = `${BASE_URI}/GetPayUrl`;
const useParkingTransactionService = () => {
    const axios = useAxios();

    const bookingSlot = (parkingTransaction) => {
        return axios.post(BOOKING_URI, parkingTransaction)
    }
    const getPaymentUrl = (parkingTransactionId) => {
        return axios.get(GET_PAY_URL + "/" + parkingTransactionId)
    }
    return {
        bookingSlot,
        getPaymentUrl
    }


}
export default useParkingTransactionService