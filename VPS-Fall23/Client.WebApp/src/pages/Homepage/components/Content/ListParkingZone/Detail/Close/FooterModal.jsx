import { Button, Space } from "antd"
import { useViewParkingZoneContext } from "../../../../../../../hooks/useContext/viewParkingZone.context"

const CloseParkingZoneFooterModal = ({ form }) => {
    const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();

    const onCloseModal = () => {
        setDetailInfo({ isShow: false, parkingZone: null, type: '' })
    }
    return (<Button onClick={onCloseModal}>Thoát</Button>)
}
export default CloseParkingZoneFooterModal