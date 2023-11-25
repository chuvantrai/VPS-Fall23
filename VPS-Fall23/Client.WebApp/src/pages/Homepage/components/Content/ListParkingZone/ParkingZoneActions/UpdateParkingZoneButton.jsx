import { Button } from "antd";
import { FormOutlined } from '@ant-design/icons'
import { useViewParkingZoneContext } from "../../../../../../hooks/useContext/viewParkingZone.context";
import ParkingZoneActionButton from "./ActionButton";
const UpdateParkingZoneButton = ({ parkingZone }) => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const button = <Button
        onClick={() => {
            setDetailInfo({ parkingZone: parkingZone, isShow: true, type: 'update' })
        }}
        icon={<FormOutlined />}
    >
    </Button>
    return (
        <ParkingZoneActionButton
            title="Cập nhật thông tin nhà xe"
            actionButton={button}
        />
    )
}
export default UpdateParkingZoneButton;