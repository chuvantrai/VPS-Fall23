import { useAxios } from '@/hooks/index.js';
import { notification } from 'antd';

const useFeedbackServices = () => {
  const axios = useAxios();

  const createFeedBack = (values, parkingZoneId) => {
    axios
      .post('/api/FeedBack/CreateFeedBackParkingZone', {
        ParkingZoneId: parkingZoneId,
        Content: values.comment ?? '',
        Rate: values.rate ?? 3,
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

  const addReplyToFeedback = (input) => {
    axios.post(`api/FeedBack/AddReplyToFeedback`, input).then((res) => {
      notification.success({
        message: res.data,
      });
    });
  };

  const filterFeedback = (ownerId, pageNumber, pageSize, parkingZoneId, rate) => {
    return axios.get(`api/FeedBack/FilterFeedback`, {
      params: {
        ownerId,
        pageNumber,
        pageSize,
        parkingZoneId,
        rate,
      },
    });
  };

  return { createFeedBack, getByParkingZone, getFeedbackForOwner, addReplyToFeedback, filterFeedback };
};

export default useFeedbackServices;
