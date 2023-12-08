import { Tabs } from "antd"
import CloseParkingZoneForm from "./CloseForm"
import ParkingZoneAbsentList from "./Absents/Table"

const CloseParkingZoneTabs = ({ form }) => {

    const tabItems = [
        {
            key: '1',
            label: "Đóng cửa",
            children: <CloseParkingZoneForm form={form} />
        },
        {
            key: '2',
            label: 'Lịch đóng cửa',
            children: <ParkingZoneAbsentList />
        }
    ]


    return (<Tabs
        items={tabItems}
        destroyInactiveTabPane={true}
    />)
}
export default CloseParkingZoneTabs