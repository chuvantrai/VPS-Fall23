import { useState } from "react";
import ParkingZonePopoverList from "./List"
import ParkingZonePopover from "./Popover"
import { setListDataBookmark } from '@/stores/parkingZones/parkingZone.store.js';
import store from '@/stores/index.jsx';
import { getListBookmarkParkingZone } from '@/helpers/index.js';
import useParkingZoneService from '@/services/parkingZoneService.js';
import { BookFilled } from "@ant-design/icons";
import { useSelector } from "react-redux";

const BookmarkParkingZonePopover = () => {

    const { listDataBookmark } = useSelector((state) => state.parkingZone);
    const [visiblePopover, setVisiblePopover] = useState(false);
    const parkingZoneService = useParkingZoneService();
    const getListDataBookmark = () => {
        if (!visiblePopover) {
            let arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
            parkingZoneService.GetParkingZonesByParkingZoneIds(arrayBookmarkPzId)
                .then((data) => {
                    store.dispatch(setListDataBookmark({ listDataBookmark: data.data ?? [] }));
                });
        }
    };
    const onOpenChangeTooltipBookmark = (value) => {
        setVisiblePopover(value);
    }
    const buttonProps = {
        onClick: getListDataBookmark,
        icon: <BookFilled />
    }
    const popoverProps = {
        title: 'Danh sách nhà xe đã được lưu',
        className: 'ml-4',
        content: <ParkingZonePopoverList parkingZones={listDataBookmark} />,
        onOpenChange: onOpenChangeTooltipBookmark
    }
    return (
        <ParkingZonePopover
            buttonProps={buttonProps}
            popoverProps={popoverProps}
        />
    )
}
export default BookmarkParkingZonePopover