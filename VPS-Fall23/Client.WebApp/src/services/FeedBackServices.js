import { useAxios } from '@/hooks/index.js';
import { notification } from 'antd';

const useFeedbackServices = () => {
  const axios = useAxios();

  const createFeedBack = (values, parkingZoneId) => {
    axios
      .post('/api/FeedBack/CreateFeedBackParkingZone', {
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
    return axios.get(`api/FeedBack/GetFeedbacksByParkingZone/${parkingZoneId}?page=${page}&pageSize=${pageSize}`);
  };

  const getFeedbackForOwner = (ownerId, pageNumber, pageSize) => {
    return axios.get(`api/FeedBack/GetFeedbackForOwner`, {
      params: {
        ownerId,
        pageNumber,
        pageSize,
      },
    });
  };

  return { createFeedBack, getByParkingZone, getFeedbackForOwner };
};

export default useFeedbackServices;
