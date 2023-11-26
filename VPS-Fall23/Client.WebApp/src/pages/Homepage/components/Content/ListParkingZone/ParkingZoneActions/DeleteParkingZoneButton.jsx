import { Button, Popconfirm, notification } from 'antd';
import { DeleteOutlined } from '@ant-design/icons';

import ParkingZoneActionButton from './ActionButton';
import { useViewParkingZoneContext } from '@/hooks/useContext/viewParkingZone.context';
import useParkingZoneService from '@/services/parkingZoneService';

function DeleteParkingZoneButton({ parkingZone }) {
  const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();
  const service = useParkingZoneService();

  const confirm = () => {
    service.deleteParkingZone(parkingZone.id).then((res) => {
      notification.success({
        message: res.data,
      });
      setViewValues({ ...viewValues, reload: true });
    });
  };

  const button = (
    <Popconfirm
      title="Xóa nhà xe"
      description="Bạn có chắc muốn xóa nhà xe này?"
      onConfirm={() => {
        confirm();
      }}
      okText="Xóa"
      cancelText="Hủy"
    >
      <Button
        type="primary"
        danger
        onClick={() => setDetailInfo({ parkingZone: parkingZone, isShow: true, type: 'delete' })}
        icon={<DeleteOutlined />}
        disabled={parkingZone.isApprove === true}
      />
    </Popconfirm>
  );

  return <ParkingZoneActionButton title="Xóa nhà xe" actionButton={button} />;
}

export default DeleteParkingZoneButton;
