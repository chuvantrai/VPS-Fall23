import { useEffect, useState } from "react";
import ParkingZoneDetailModal from "../Modal"
import ParkingZoneDescriptionCarousel from "./Carousel";
import ParkingZoneDescription from "./Description";
import ParkingZoneDescriptionFooterModal from "./FooterModal";
import { useViewParkingZoneContext } from "../../../../../../../hooks/useContext/viewParkingZone.context";
import useParkingZoneService from '@/services/parkingZoneService';

const ParkingZoneDescriptionModal = () => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();

    const parkingZoneService = useParkingZoneService();


    const modalProps = {
        width: '40vw',
        open: detailInfo.isShow,
        title: "Thông tin bãi đỗ xe",
        footer: <ParkingZoneDescriptionFooterModal />
    }
    const [parkingZoneImages, setParkingZoneImages] = useState([])
    useEffect(() => {
        parkingZoneService.getImageLink(detailInfo.parkingZone.id).then((res) => setParkingZoneImages(res.data));
    }, [])

    const modalContent = <>
        <ParkingZoneDescriptionCarousel parkingZoneImages={parkingZoneImages} />
        <ParkingZoneDescription parkingZone={detailInfo.parkingZone} />
    </>
    return (
        <ParkingZoneDetailModal
            parkingZone={detailInfo.parkingZone}
            modalContent={modalContent}
            modalProps={modalProps}
        />
    )
}
export default ParkingZoneDescriptionModal