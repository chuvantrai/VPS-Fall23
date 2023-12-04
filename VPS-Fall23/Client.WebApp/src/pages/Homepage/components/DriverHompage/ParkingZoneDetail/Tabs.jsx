import { Tabs } from "antd";
import BookingForm from "./Booking/BookingForm";
import FeedBackForm from "./Feedback/FeedBackForm";
import FeedbackList from "./Feedback/FeedbackList";
import Information from "./Information";
import ParkingZoneDetailBookmark from "./Bookmark";
import { useParkingZoneDetailContext } from "@/hooks/useContext/driver.parkingZoneDetail.context";

const ParkingZoneDetailTabs = ({ parkingZone }) => {
    const { detailFormInfo, setDetailFormInfo } = useParkingZoneDetailContext()
    const items = [
        {

            key: '1',
            label: 'Chi tiết',
            children: (<Information parkingZone={parkingZone}></Information>),
        },
        {
            key: '2',
            label: 'Xem đánh giá',
            children: <FeedbackList parkingZoneId={parkingZone?.id} />,
        },
        {
            key: '3',
            label: 'Đặt chỗ',
            children: (<BookingForm
                parkingZone={parkingZone}

            ></BookingForm>),
        },
        {
            key: '4',
            label: 'Viết đánh giá',
            children: (<FeedBackForm
                parkingZoneId={parkingZone?.id}
            />),
        },
    ];

    const onChangeTabs = (key) => {
        setDetailFormInfo({ ...detailFormInfo, tab: key })
    };
    return (<div className={('pt-[10px]')}>
        <Tabs
            activeKey={detailFormInfo.tab}
            items={items}
            destroyInactiveTabPane={true}
            onChange={onChangeTabs}
            tabBarExtraContent={<ParkingZoneDetailBookmark parkingZone={parkingZone} />} />
    </div>)
}
export default ParkingZoneDetailTabs