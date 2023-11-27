import { DeleteRowOutlined } from "@ant-design/icons"
import { Button, Space, Tooltip, notification } from "antd"
import useParkingZoneAbsentServices from "@/services/parkingZoneAbsentServices";

const ParkingZoneAbsentActions = ({ record, reloadCallback }) => {
    const parkingZoneAbsentService = useParkingZoneAbsentServices();
    const onDelete = () => {
        parkingZoneAbsentService.deleteAbsent(record.id).then(res => {
            notification.success({ message: 'Thành công', description: "Hủy lịch đóng cửa thành công" })
            reloadCallback();
        })
    }
    const deleteButton = (<Tooltip title="Hủy lịch">
        <Button
            icon={<DeleteRowOutlined />}
            onClick={onDelete}
        />
    </Tooltip>)
    return <Space.Compact>
        {deleteButton}
    </Space.Compact>
}
export default ParkingZoneAbsentActions