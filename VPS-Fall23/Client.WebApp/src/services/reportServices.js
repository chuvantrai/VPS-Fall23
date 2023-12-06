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

  const getReportForAdmin = (pageNumber, pageSize) => {
    return axios.get(`api/Report/GetReportForAdmin`, {
      params: {
        pageNumber,
        pageSize,
      },
    });
  };

  const loadReportType = () => {
    return axios.get(`api/Report/GetTypeReport`);
  };

  const filterReport = (pageNumber, pageSize, typeId) => {
    return axios.get(`api/Report/FilterReport`, {
      params: {
        pageNumber,
        pageSize,
        typeId,
      },
    });
  };
  const updateStatusReport = (reportId, statusId) => {
    return axios.post(`api/Report/UpdateStatusReport`, null, {
      params: {
        reportId,
        statusId,
      },
    });
  };
  return { createReport, getReportForAdmin, loadReportType, filterReport, updateStatusReport };
};

export default reportServices;
