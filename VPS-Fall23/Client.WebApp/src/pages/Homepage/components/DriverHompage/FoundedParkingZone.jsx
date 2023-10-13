import { PicCenterOutlined } from '@ant-design/icons'
import { Button, Descriptions, List, Popover } from "antd";
import { useSelector } from "react-redux";
import ButtonGroup from 'antd/es/button/button-group';

const FoundedParkingZone = ({ viewOnThisMapCallback, viewDetailCallback, viewOnGoogleMapCallback, bookingCallback }) => {
    const { listFounded } = useSelector(state => state.parkingZone);
    // useMemo(() => {
    //     setListOpen(true);
    // }, [JSON.stringify(listFounded)])

    const getFullAddress = (parkingZone) =>
        (`${parkingZone.detailAddress}, ${parkingZone.commune.name}, ${parkingZone.commune.district.name}, ${parkingZone.commune.district.city.name}`)
    const getDescriptionItem = (parkingZone) => (
        [
            {
                key: 2,
                label: "Số lượng trống",
                children: parkingZone.slots ?? 0
            },
            {
                key: 3,
                label: "Giá/giờ",
                children: <p>{parkingZone.pricePerHour} VNĐ</p>
            },
            {
                key: 4,
                label: "Địa chỉ",
                children: getFullAddress(parkingZone)
            }
        ]
    )
    const getDesciptionTitle = (parkingZone) => (
        <>
            <span>{parkingZone.name}: </span>
            <ButtonGroup>
                <Button onClick={() => viewOnThisMapCallback(parkingZone)}>Xem địa điểm</Button>
                <Button onClick={() => viewDetailCallback(parkingZone)}>Chi tiết nhà xe</Button>
                <Button onClick={() => viewOnGoogleMapCallback(parkingZone)}>Xem trên google maps</Button>
                <Button onClick={() => bookingCallback(parkingZone)} type="primary" danger>Đặt vé</Button>
            </ButtonGroup>
        </>
    )
    const getFoundedParkingZonePopupContent = () => {
        return (<List
            pagination={{
                position: "top",
                align: "center",
                size: "small",
                pageSize: 2,
                total: listFounded.length
            }}
            dataSource={listFounded}
            renderItem={(val, index) => {
                return (<List.Item key={index}>
                    <Descriptions
                        title={getDesciptionTitle(val)}
                        size="small"
                        items={getDescriptionItem(val)}
                        bordered={true}
                        column={2}
                    />
                </List.Item>)
            }}
        >
        </List>)
    }
    return (<Popover
        trigger={"click"}
        content={getFoundedParkingZonePopupContent}
        placement="bottomLeft"
        title="Danh sách nhà xe đã tìm được">
        <Button type="primary" danger

            icon={<PicCenterOutlined />}></Button>
    </Popover>)
}
export default FoundedParkingZone