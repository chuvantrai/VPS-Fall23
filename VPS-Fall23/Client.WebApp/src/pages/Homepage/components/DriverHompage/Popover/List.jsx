import { List } from "antd";
import ParkingZonePopoverDescription from "./Description";


const ParkingZonePopoverList = ({ parkingZones }) => {

    const pagination = {
        position: 'top',
        align: 'center',
        size: 'small',
        pageSize: 2,
        total: parkingZones.length,
    }
    const getListItems = (value, index) => {
        return (<List.Item key={index}>
            <ParkingZonePopoverDescription parkingZone={value} />
        </List.Item>)
    }

    return (<List
        pagination={pagination}
        dataSource={parkingZones}
        renderItem={getListItems}
    ></List>)
}
export default ParkingZonePopoverList