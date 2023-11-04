import { useAxios } from '@/hooks/index.js';
import { notification } from 'antd';

const feedBackServices = () => {

  const axios = useAxios();

  const createFeedBack = (values, parkingZoneId) => {
    axios.post('/api/FeedBack/CreateFeedBackParkingZone', {
      ParkingZoneId: parkingZoneId,
      Content: values.comment ?? '',
      Rate: values.rate,
      Email: values.email,
    })
      .then(() => {
        notification.success({
          message: 'Thành công',
        });
      });
  };
  const getByParkingZone = (parkingZoneId, page, pageSize) => {
    return axios.get(`api/FeedBack/GetFeedbacksByParkingZone/${parkingZoneId}?page=${page}&pageSize=${pageSize}`)
  }
  return { createFeedBack, getByParkingZone };
};

export default feedBackServices;