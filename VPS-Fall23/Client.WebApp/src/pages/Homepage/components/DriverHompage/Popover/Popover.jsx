import { Button, Popover } from "antd"

const ParkingZonePopover = ({ popoverProps, buttonProps }) => {
    return (<Popover
        {...popoverProps}
        trigger={'click'}
        placement='bottomLeft'

    >
        <Button
            {...buttonProps}
            type='primary'
        >
        </Button>
    </Popover>)
}
export default ParkingZonePopover