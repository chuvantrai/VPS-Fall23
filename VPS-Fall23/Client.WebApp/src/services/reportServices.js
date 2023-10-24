import { useAxios } from '@/hooks/index.js';
import { notification } from 'antd';

const reportServices = () => {

  const axios = useAxios();

  const createReport = (values) => {
    axios
      .post('/api/Report/CreateReport', {
        content: values.content,
        email: values.email ?? '',
        type: values.type,
        paymentCode: values.paymentCode ?? '',
        phone: values.phone ?? '',
      })
      .then(() => {
        notification.success({
          message: 'Thành công',
        });
      });
  };
  return { createReport };
};

export default reportServices;