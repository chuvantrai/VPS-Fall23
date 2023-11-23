import { BookOutlined, BookTwoTone } from "@ant-design/icons";
import { Button, Tooltip, notification } from "antd";
import { useEffect, useState } from "react";
import { getListBookmarkParkingZone, setListBookmarkParkingZone } from '@/helpers/index.js';
const ParkingZoneDetailBookmark = ({ parkingZone }) => {
    const [isBookmark, setIsBookmark] = useState(false);
    const clickBookMark = () => {
        let arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
        if (!isBookmark) {
            arrayBookmarkPzId.push(parkingZone.id);
            setIsBookmark(true);
            notification.success({
                message: 'Lưu thành công',
            });
        } else {
            arrayBookmarkPzId = arrayBookmarkPzId.filter(item => item !== parkingZone.id);
            setIsBookmark(false);
            notification.success({
                message: 'Bỏ lưu thành công',
            });
        }
        setListBookmarkParkingZone(arrayBookmarkPzId);
    };

    useEffect(() => {
        const arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
        setIsBookmark(arrayBookmarkPzId.includes(parkingZone?.id));
    }, [parkingZone?.id]);

    return (<>
        <Tooltip placement='bottom' title={isBookmark ? 'Bỏ lưu bãi đỗ xe' : 'Lưu bãi đỗ xe'} zIndex={10000}>
            <Button icon={isBookmark ? <BookTwoTone /> : <BookOutlined />} onClick={clickBookMark}></Button>
        </Tooltip>
    </>);
}
export default ParkingZoneDetailBookmark