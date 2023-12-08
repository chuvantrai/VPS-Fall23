import { useForm } from "antd/es/form/Form";
import { useViewParkingZoneContext } from "../../../../../../../hooks/useContext/viewParkingZone.context";
import ParkingZoneDetailModal from "../Modal";
import CloseParkingZoneTabs from "./CloseTabs";

const CloseParkingZoneModal = () => {

    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const [form] = useForm();
    const modalProps = {
        open: detailInfo.isShow,
        title: "Đóng cửa bãi đỗ xe",
        okText: "Đóng cửa",
        cancelText: "Thoát",
        onCancel: () => setDetailInfo({ isShow: false, parkingZone: null, type: '' }),
        footer: <></> //<CloseParkingZoneFooterModal form={form} />
    }

    return (
        <ParkingZoneDetailModal
            parkingZone={detailInfo.parkingZone}
            modalProps={modalProps}
            modalContent={<CloseParkingZoneTabs form={form} />}
        />
    )
}
export default CloseParkingZoneModal;