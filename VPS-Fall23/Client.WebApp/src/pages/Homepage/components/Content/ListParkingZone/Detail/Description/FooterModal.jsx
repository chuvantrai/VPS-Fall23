import { Button, Space, Switch } from "antd";
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";
import useParkingZoneService from '@/services/parkingZoneService';
import { useEffect, useState } from "react";
import getAccountJwtModel from "@/helpers/getAccountJwtModel";
const ParkingZoneDescriptionFooterModal = () => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const parkingZoneService = useParkingZoneService();

    const [switchChecked, setSwitchChecked] = useState(detailInfo.parkingZone.isFull)
    const [account, setAccount] = useState();

    const handleCancel = () => {
        setDetailInfo({ isShow: false, parkingZone: null, type: '' })
    };
    const onSwitchChange = (checked) => {
        const params = {
            parkingZoneId: detailInfo.parkingZone.id,
            isFull: checked,
        };
        parkingZoneService.changeParkingZoneFullStatus(params);
        setSwitchChecked(!switchChecked)
    }
    useEffect(() => {
        setAccount(getAccountJwtModel())
    }, [])
    return (<Space>
        {account?.RoleId === '2' && (
            <Switch
                checkedChildren="Hết chỗ"
                unCheckedChildren="Còn chỗ"
                onChange={onSwitchChange}
                checked={switchChecked}
            />
        )}
        <Button type="dashed" onClick={handleCancel}>
            Đóng
        </Button>
    </Space>)
}
export default ParkingZoneDescriptionFooterModal;