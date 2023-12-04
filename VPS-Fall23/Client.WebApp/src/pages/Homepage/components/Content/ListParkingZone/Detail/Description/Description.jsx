import { Badge, Descriptions, Tag } from "antd";
import { useEffect, useState } from "react";
import useParkingZoneAbsentServices from '@/services/parkingZoneAbsentServices';
import dayjs from "dayjs";

const ParkingZoneDescription = ({ parkingZone }) => {
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
    });
    const [parkingZoneAbsents, setParkingZoneAbsents] = useState([]);
    const parkingZoneAbsentService = useParkingZoneAbsentServices();
    useEffect(() => {
        parkingZoneAbsentService
            .getAbsents(parkingZone.id)
            .then(res => setParkingZoneAbsents(res.data))
    }, [])
    const isActive = () => {
        const isClosing = parkingZoneAbsents.some(pa => pa.from <= dayjs().startOf('day') && pa.to && pa.to >= dayjs().endOf('day'))
        if (isClosing) {
            return <Badge status="error" text="Ngừng hoạt động" />;
        }
        return <Badge status="success" text="Đang hoạt động" />;
    };

    const descItems = [
        {
            key: '1',
            label: 'Tên',
            children: parkingZone.name,
        },
        {
            key: '2',
            label: 'Ngày tạo',
            children: dayjs(parkingZone.createdAt).format('HH:mm DD/MM/YYYY'),
        },
        {
            key: '3',
            label: 'Giá tiền (mỗi tiếng)',
            children: formatter.format(parkingZone.pricePerHour),
        },
        {
            key: '4',
            label: 'Giá tiền quá giờ (mỗi tiếng)',
            children: formatter.format(parkingZone.priceOverTimePerHour),
        },
        {
            key: '5',
            label: 'Số chỗ tối đa',
            children: <Tag color="processing">{parkingZone.slots}</Tag>,
        },
        {
            key: '6',
            label: 'Trạng thái',
            children: isActive(),
        },
        {
            key: '7',
            label: 'Thời gian làm việc',
            children: parkingZone.workFrom + ' - ' + parkingZone.workTo,
            span: 2,
        },
        {
            key: '8',
            label: 'Địa chỉ chi tiết',
            children: parkingZone.detailAddress,
            span: 2,
        },
    ];


    return (
        <Descriptions bordered items={descItems} column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }} />)
}
export default ParkingZoneDescription;