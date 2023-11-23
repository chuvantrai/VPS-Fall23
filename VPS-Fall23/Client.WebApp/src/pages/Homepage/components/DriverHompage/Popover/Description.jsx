import { Button, Descriptions } from "antd"
import { useParkingZoneDetailContext } from "@/hooks/useContext/driver.parkingZoneDetail.context";

const ParkingZonePopoverDescription = ({ parkingZone, title = "" }) => {

    const { detailFormInfo, setDetailFormInfo } = useParkingZoneDetailContext();
    const viewDetailInfo = ({ parkingZone, tab = '1' }) => {
        setDetailFormInfo({ isShow: true, parkingZone: parkingZone, tab: tab })
    }
    const viewOnMap = (parkingZone) => {

    }
    const descriptionItem = [
        {
            key: 1,
            span: 2,
            label: <Button onClick={() => viewDetailInfo({ parkingZone: parkingZone, tab: '3' })} type='primary' danger>Đặt vé</Button>,
            children: <Button onClick={() => viewDetailInfo({ parkingZone: parkingZone })}>{parkingZone.name}</Button>,
        },
        {
            key: 2,
            label: 'Số lượng xe tối đa',
            children: parkingZone.slots ?? 0,
        },
        {
            key: 3,
            label: 'Giá/giờ',
            children: <p>{parkingZone.pricePerHour} VNĐ</p>,
        },
        {
            key: 4,
            label: 'Địa chỉ',
            children: <Button onClick={() => viewOnMap(parkingZone)}> {parkingZone.detailAddress}</Button>,
        }
    ]
    return <Descriptions
        title={title}
        size='small'
        items={descriptionItem}
        bordered={true}
        column={2}
    />
}
export default ParkingZonePopoverDescription