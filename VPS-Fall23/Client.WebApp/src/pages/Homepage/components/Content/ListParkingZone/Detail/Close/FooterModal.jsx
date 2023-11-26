import { Button, Popconfirm, Space } from "antd"
import { QuestionCircleOutlined } from "@ant-design/icons"
import { useViewParkingZoneContext } from "../../../../../../../hooks/useContext/viewParkingZone.context"
import useParkingZoneService from '@/services/parkingZoneService';
import { useState } from "react";

const CloseParkingZoneFooterModal = ({ form }) => {
    const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();

    const parkingZoneService = useParkingZoneService();
    const [popConfirmOpen, setPopConfirmOpen] = useState(false);
    const onCloseModal = () => {
        setDetailInfo({ isShow: false, parkingZone: null, type: '' })
    }
    const onCloseClick = () => {
        form.validateFields()
            .then(() => setPopConfirmOpen(true))
    }
    const submit = (values) => {
        console.log(values);
        const input = {
            ...values,
            parkingZoneId: detailInfo.parkingZone.id,
            closeFrom: Array.isArray(values.closeTime) === true ? values.closeTime[0] : values.closeTime,
            closeFrom: Array.isArray(values.closeTime) === true ? values.closeTime[1] : null
        }
        setPopConfirmOpen(false)
        parkingZoneService.closeParkingZone(input).then(res => setViewValues({ ...viewValues, reload: true }));


    }
    return (<Space>
        <Button type="dashed" onClick={onCloseModal}>Thoát</Button>

        <Popconfirm
            title="Đóng cửa bãi đỗ xe"
            description="Bạn có chắc muốn đóng cửa bãi đỗ xe này ?"
            onConfirm={() => submit(form.getFieldsValue())}
            onCancel={() => setPopConfirmOpen(false)}
            cancelText="Hủy"
            okText="Xác nhận đóng cửa"
            open={popConfirmOpen}
        >
            <Button type='primary' onClick={onCloseClick}>Đóng cửa</Button>
        </Popconfirm>
    </Space>)
}
export default CloseParkingZoneFooterModal