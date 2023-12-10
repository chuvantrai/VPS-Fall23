import { Table } from 'antd';
import ParkingZoneApproveStatus from '../ParkingZoneApproveStatus';
import ParkingZoneActions from '../ParkingZoneActions';
import dayjs from 'dayjs';

const ParkingZonesTable = ({ parkingZones }) => {
  const formatter = new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  });
  const columns = [
    {
      title: 'Tên bãi đỗ xe',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Chủ bãi đỗ xe',
      dataIndex: 'owner',
      key: 'owner',
      render: (value, record) => {
        const fullName = `${record.owner?.idNavigation?.firstName ?? ''} ${record.owner?.idNavigation?.lastName ?? ''}`;
        return fullName.trim() ?? value;
      },
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (val) => dayjs(val).format(' HH:mm DD/MM/YYYY'),
    },
    {
      title: 'Đơn giá/giờ  (VNĐ)',
      dataIndex: 'pricePerHour',
      key: 'pricePerHour',
      render: (value) => formatter.format(value),
    },
    {
      title: 'Đơn giá quá giờ/giờ (VNĐ)',
      dataIndex: 'priceOverTimePerHour',
      key: 'priceOverTimePerHour',
      render: (value) => formatter.format(value),
    },
    {
      title: 'Số chỗ tối đa',
      dataIndex: 'slots',
      key: 'slots',
    },
    {
      title: 'Giờ làm việc',
      dataIndex: 'workFrom',
      key: 'workFrom',
      render: (_, record) => `${record.workFrom.split(':')[0]}h - ${record.workTo.split(':')[0]}h`,
    },
    {
      title: 'Trạng thái duyệt',
      dataIndex: 'isApprove',
      key: 'isApprove',
      render: (val) => <ParkingZoneApproveStatus isApprove={val} />,
    },

    {
      title: '',
      key: 'action',
      render: (_, record) => <ParkingZoneActions parkingZone={record} />,
    },
  ];
  return <Table bordered columns={columns} dataSource={parkingZones} pagination={false} />;
};
export default ParkingZonesTable;
