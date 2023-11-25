import { Tooltip } from "antd"

const ParkingZoneActionButton = ({ title, actionButton }) => {

    return (<Tooltip title={title}>{actionButton}</Tooltip>)
}
export default ParkingZoneActionButton