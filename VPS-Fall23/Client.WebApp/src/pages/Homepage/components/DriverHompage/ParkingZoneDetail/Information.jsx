
import useParkingZoneService from '@/services/parkingZoneService.js';
import { ReloadOutlined } from '@ant-design/icons';
import { Button, Descriptions, Divider, Space, Tag } from 'antd';
import { useEffect, useState } from 'react';
const Information = ({ parkingZone }) => {
    const onGetFreeSlot = (parkingZoneId) => {
        parkingZoneService.getBookedSlot(parkingZoneId).then(res => setFreeSlots(parkingZone.slots - res.data));
    };
    const [freeSlots, setFreeSlots] = useState(parkingZone?.slots ?? 0);
    useEffect(() => {
        onGetFreeSlot(parkingZone.id)
    }, [])
    const parkingZoneService = useParkingZoneService();

    const getDetailDescription = () => {
        if (!parkingZone) return [];
        return [
            {
                key: 1,
                label: 'Địa chỉ',
                children: parkingZone.detailAddress,
            },
            {
                key: 2,
                label: 'Xã/Phường',
                children: parkingZone.commune.name,
            },
            {
                key: 3,
                label: 'Quận/Huyện',
                children: parkingZone.commune.district.name,
            },
            {
                key: 4,
                label: 'Tỉnh/Thành phố',
                children: parkingZone.commune.district.city.name,
            },
            {
                key: 5,
                label: 'Giá thành mỗi giờ (VNĐ)',
                children: parkingZone.pricePerHour ?? 0,
            },
            {
                key: 6,
                label: 'Giá khi đỗ quá giờ (VNĐ)',
                children: parkingZone.priceOverTimePerHour ?? 0,
            },
            {
                key: 5,
                label: (
                    <Space>
                        <p>Số chỗ trống</p>
                        <Button onClick={() => onGetFreeSlot(parkingZone.id)}
                            icon={<ReloadOutlined />}
                        ></Button>
                    </Space>
                ),
                children: (
                    (<Space>
                        <Tag color="processing">{freeSlots ?? parkingZone.slots}</Tag>
                    </Space>)
                ),
            },
        ];
    };
    return (<Descriptions
        bordered
        items={getDetailDescription()}
        size='small'
        column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }}
    />)
}
export default Information