import { PicCenterOutlined } from "@ant-design/icons"
import ParkingZonePopover from "./Popover"
import ParkingZonePopoverList from "./List"
import { useState } from "react";
import { Input } from "antd";


const FoundedParkingZonePopover = ({ parkingZones }) => {

    const [searchValue, setSearchValue] = useState();
    const getListFounded = () => {
        if (!searchValue) return parkingZones;
        return parkingZones.filter(val => {
            return val.detailAddress.toLowerCase().trim().includes(searchValue.toLowerCase().trim())
                || val.name.toLowerCase().trim().includes(searchValue.toLowerCase().trim());
        });
    };
    const buttonProps = {
        danger: true,
        icon: <PicCenterOutlined />
    }
    const popoverProps = {
        title: 'Danh sách nhà xe đã tìm được',
        content: (
            <>
                <Input
                    type='search'
                    placeholder='Nhập để tìm nhà xe...'
                    onChange={({ target }) => setSearchValue(target.value)}
                />
                <ParkingZonePopoverList parkingZones={getListFounded()} />
            </>
        )
    }
    return (<ParkingZonePopover
        buttonProps={buttonProps}
        popoverProps={popoverProps}
    />)
}
export default FoundedParkingZonePopover