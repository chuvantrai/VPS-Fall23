import { Divider, Space } from "antd";
import CloseParkingZoneButton from "./CloseParkingZoneButton";
import UpdateAddressButton from "./UpdateAddressButton";
import UpdateParkingZoneButton from "./UpdateParkingZoneButton";

const ParkingZoneActions = ({ parkingZone }) => {
    return (<Space split={<Divider type="vertical" />}>
        <UpdateParkingZoneButton parkingZone={parkingZone} />
        <UpdateAddressButton parkingZone={parkingZone} />
        <CloseParkingZoneButton parkingZone={parkingZone} />
    </Space>)
}
export default ParkingZoneActions