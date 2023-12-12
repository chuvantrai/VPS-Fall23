import { useAxios } from '@/hooks';

const BASE_URI = 'api/ParkingTransaction';
const BOOKING_URI = `${BASE_URI}/Booking`;
const GET_PAY_URL = `${BASE_URI}/GetPayUrl`;
const GET_ALL_INCOME = `${BASE_URI}/GetAllIncome`;
const useParkingTransactionService = () => {
  const axios = useAxios();

  const bookingSlot = (parkingTransaction) => {
    return axios.post(BOOKING_URI, parkingTransaction);
  };
  const getPaymentUrl = (parkingTransactionId) => {
    return axios.get(GET_PAY_URL + '/' + parkingTransactionId);
  };

  const getAllIncome = (parkingZoneId, ownerId) => {
    console.log(ownerId);
    return axios.get(GET_ALL_INCOME, {
      params: { parkingZoneId, ownerId },
    });
  };
  return {
    bookingSlot,
    getPaymentUrl,
    getAllIncome,
  };
};
export default useParkingTransactionService;
