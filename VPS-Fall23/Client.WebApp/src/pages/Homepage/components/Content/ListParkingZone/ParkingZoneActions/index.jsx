import { Space } from 'antd';
import CloseParkingZoneButton from './CloseParkingZoneButton';
import UpdateAddressButton from './UpdateAddressButton';
import UpdateParkingZoneButton from './UpdateParkingZoneButton';
import DeleteParkingZoneButton from './DeleteParkingZoneButton';

const ParkingZoneActions = ({ parkingZone }) => {
  return (
    <Space.Compact>
      <UpdateParkingZoneButton parkingZone={parkingZone} />
      <UpdateAddressButton parkingZone={parkingZone} />
      <CloseParkingZoneButton parkingZone={parkingZone} />
      <DeleteParkingZoneButton parkingZone={parkingZone} />
    </Space.Compact>
  );
};
export default ParkingZoneActions;
