import { Button, Space, Switch } from "antd";
import { useState } from "react";
import useParkingZoneService from '@/services/parkingZoneService';
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";
import dayjs from "dayjs";

const UpdateParkingZoneFooterModal = ({ form }) => {
    const parkingZoneService = useParkingZoneService();
    const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();
    const [switchChecked, setSwitchChecked] = useState(detailInfo.parkingZone.isFull)

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
        setViewValues({ ...viewValues, reload: true })
    }

    return (<Space>
        <Switch
            checkedChildren="Hết chỗ"
            unCheckedChildren="Còn chỗ"
            onChange={onSwitchChange}
            checked={switchChecked}
        />
        <Button type="dashed" onClick={handleCancel}>
            Đóng
        </Button>
        <Button
            type="primary"
            onClick={() => {
                form.validateFields().then((values) => {
                    console.log(values);
                    const formData = new FormData();
                    formData.append('parkingZoneId', detailInfo.parkingZone.id);
                    formData.append('parkingZoneName', values.name);
                    formData.append('pricePerHour', values.pricePerHour);
                    formData.append('priceOverTimePerHour', values.priceOverTimePerHour);
                    formData.append('slots', values.slots);
                    formData.append('workFrom', dayjs(values.workingTime[0]).format("HH:mm:ss"));
                    formData.append('workTo', dayjs(values.workingTime[1]).format("HH:mm:ss"));
                    if (values.parkingZoneImages) {
                        values.parkingZoneImages.fileList.forEach((item) => {
                            formData.append('parkingZoneImages', item.originFileObj);
                        });
                    }


                    parkingZoneService.updateParkingZone(formData).then((res) => {
                        setViewValues({ ...viewValues, reload: true })
                        handleCancel();
                    });
                }).catch(err => console.log(err));
            }}
        >
            Lưu
        </Button>
    </Space >)
}
export default UpdateParkingZoneFooterModal