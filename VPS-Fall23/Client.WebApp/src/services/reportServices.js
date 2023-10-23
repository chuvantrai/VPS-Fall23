import { useAxios } from '@/hooks/index.js';
import { notification } from 'antd';

const reportServices = () => {

  const axios = useAxios();
  
  const createReport = (values, parkingZoneId) => {
    // axios
    //   .post('/api/FeedBack/CreateFeedBackParkingZone', {
    //     ParkingZoneId: parkingZoneId,
    //     Content: values.comment??"",
    //     Rate: values.rate,
    //     Email: values.email
    //   })
    //   .then(() => {
    //     notification.error({
    //       message: 'Thành công'
    //     });
    //   })
    //   .catch((err) => {
    //     notification.error({
    //       message: 'Lỗi',
    //       description: err,
    //     });
    //   });
  }
}

export default reportServices;