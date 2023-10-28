import reportTypeEnum from '@/helpers/reportTypeEnum.js';

const optionsReportType = [
  { value: reportTypeEnum.ERROR_DISPLAY, label: 'Lỗi hiện thị' },
  { value: reportTypeEnum.SYSTEM_ERROR, label: 'Lỗi hệ thống' },
  { value: reportTypeEnum.TRANSACTION_ERROR, label: 'Lỗi giao dịch' },
  { value: reportTypeEnum.REQUEST_TRANSACTION_REFUND, label: 'Yêu cầu hoàn tiền giao dịch' },
  // { value: reportTypeEnum.REPORT_PARKING_ZONE, label: 'Tố cáo bãi đỗi xe' },
];
export default optionsReportType;