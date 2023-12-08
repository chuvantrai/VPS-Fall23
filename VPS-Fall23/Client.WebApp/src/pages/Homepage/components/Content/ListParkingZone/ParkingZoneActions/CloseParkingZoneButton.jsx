import { Button } from "antd";
import { PauseCircleOutlined } from '@ant-design/icons'
import { useViewParkingZoneContext } from "../../../../../../hooks/useContext/viewParkingZone.context";
import ParkingZoneActionButton from "./ActionButton";

const CloseParkingZoneButton = ({ parkingZone }) => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const button = <Button
        danger
        onClick={() => setDetailInfo({ parkingZone: parkingZone, isShow: true, type: 'close' })}
        icon={<PauseCircleOutlined />}
    />
    return (
        <ParkingZoneActionButton
            title="Đóng cửa nhà xe"
            actionButton={button}
        />
    )
}
export default CloseParkingZoneButton