import { useForm } from "antd/es/form/Form";
import { useViewParkingZoneContext } from "../../../../../../../hooks/useContext/viewParkingZone.context";
import ParkingZoneDetailModal from "../Modal";
import CloseParkingZoneForm from "./CloseForm";
import CloseParkingZoneFooterModal from "./FooterModal";

const CloseParkingZoneModal = ({ parkingZone }) => {

    const { detailInfo } = useViewParkingZoneContext();
    const [form] = useForm();
    const modalProps = {
        open: detailInfo.isShow,
        title: "Đóng cửa bãi đỗ xe",
        okText: "Đóng cửa",
        cancelText: "Thoát",
        footer: <CloseParkingZoneFooterModal form={form} />
    }

    return (
        <ParkingZoneDetailModal
            parkingZone={detailInfo.parkingZone}
            modalProps={modalProps}
            modalContent={<CloseParkingZoneForm form={form} />}
        />
    )
}
export default CloseParkingZoneModal;