import { Button } from "antd";
import { EnvironmentOutlined } from '@ant-design/icons'
import { useViewParkingZoneContext } from "../../../../../../hooks/useContext/viewParkingZone.context";
import ParkingZoneActionButton from "./ActionButton";

const UpdateAddressButton = ({ parkingZone }) => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const button = <Button
        icon={<EnvironmentOutlined />}
        onClick={() => { setDetailInfo({ isShow: true, parkingZone: parkingZone, type: 'address' }) }}
    >
    </Button>
    return (
        <ParkingZoneActionButton
            title="Cập nhật địa chỉ nhà xe"
            actionButton={button}
        />
    )
}
export default UpdateAddressButton