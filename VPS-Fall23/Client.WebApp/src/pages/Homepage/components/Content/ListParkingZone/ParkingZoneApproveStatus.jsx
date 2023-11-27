import { Tag } from "antd"
const approveStatuses = [
    {
        label: "Đang duyệt",
        color: "processing",
        value: null
    },
    {
        label: "Đã từ chối",
        color: "red",
        value: false
    },
    {
        label: "Đã duyệt",
        color: "success",
        value: true
    }
]
const ParkingZoneApproveStatus = ({ isApprove }) => {

    const approveStatus = approveStatuses.find((as) => as.value === isApprove);
    if (isApprove === undefined) return <></>

    return (<Tag color={approveStatus.color}>
        <a>{approveStatus.label}</a>
    </Tag>)
}
export default ParkingZoneApproveStatus;