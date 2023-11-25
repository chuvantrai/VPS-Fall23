import ParkingZoneDetailModal from "../Modal"
import UpdateParkingZoneFooterModal from "./FooterModal";
import UpdateParkingZoneForm from "./UpdateForm"
import { useForm } from "antd/es/form/Form";
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";

const UpdateParkingZoneModal = () => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const [form] = useForm();
    const handleCancel = () => {
        setDetailInfo({ isShow: false, parkingZone: null, type: '' })
    };
    const modalProps = {
        width: 600,
        open: detailInfo.isShow,
        onCancel: handleCancel,
        title: "Thông tin bãi đỗ xe",
        footer: <UpdateParkingZoneFooterModal form={form} />
    }

    return (
        <ParkingZoneDetailModal
            parkingZone={detailInfo.parkingZone}
            modalProps={modalProps}
            modalContent={<UpdateParkingZoneForm form={form} parkingZone={detailInfo.parkingZone} />}
        />
    )
}
export default UpdateParkingZoneModal