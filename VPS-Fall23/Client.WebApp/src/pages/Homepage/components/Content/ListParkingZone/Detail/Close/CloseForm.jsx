import { Button, DatePicker, Form, Input, Popconfirm, Radio, Space } from "antd"
import dayjs from "dayjs";
import { useEffect, useState } from "react"
import useParkingZoneService from '@/services/parkingZoneService';
import useParkingZoneAbsentServices from "@/services/parkingZoneAbsentServices";
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";

const CloseParkingZoneForm = ({ form }) => {

    const parkingZoneService = useParkingZoneService();
    const parkingZoneAbsentService = useParkingZoneAbsentServices();
    const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();
    const [closeType, setCloseType] = useState(2);
    const [absents, setAbsents] = useState();

    useEffect(() => {
        parkingZoneAbsentService.getAbsents(detailInfo.parkingZone.id).then(res => setAbsents(res.data))
    }, [])


    const getDisabledDate = (value) => {
        var absentToBool = absents.map((a) => {
            return (!a.from || dayjs(a.from)) <= value && (!a.to || value <= dayjs(a.to))
        })
        return value && value <= dayjs().endOf('day') || absentToBool.includes(true);
    }

    const getTimePicker = (closeType) => {
        switch (closeType) {
            case 1: return <DatePicker disabledDate={getDisabledDate} placeholder="Từ ngày"></DatePicker>;
            case 2:
            default: return <DatePicker.RangePicker disabledDate={getDisabledDate} placeholder={["Từ ngày", "Đến ngày"]}></DatePicker.RangePicker>
        }
    }
    const submit = () => {
        form.validateFields().then((values) => {
            console.log(values);
            const input = {
                ...values,
                parkingZoneId: detailInfo.parkingZone.id,
                closeFrom: Array.isArray(values[`closeTime_${closeType}`]) === true ? values[`closeTime_${closeType}`][0] : values[`closeTime_${closeType}`],
                closeTo: Array.isArray(values[`closeTime_${closeType}`]) === true ? values[`closeTime_${closeType}`][1] : null
            }
            parkingZoneService.closeParkingZone(input).then(res => {
                setViewValues({ ...viewValues, reload: true })
                form.resetFields();
            });
        })


    }

    return (
        <Form form={form} layout="vertical" name="closeModalForm">
            <Form.Item>
                <Radio.Group onChange={({ target }) => setCloseType(target.value)} value={closeType}>
                    <Space>
                        <Radio value={1} >Đóng cửa vĩnh viễn</Radio>
                        <Radio value={2} >Đóng cửa một thời gian</Radio>
                    </Space>
                </Radio.Group>
            </Form.Item>

            <Form.Item
                name={`closeTime_${closeType}`}
                label="Thời gian đóng cửa"
                rules={[
                    {
                        required: true,
                        message: "Vui lòng chọn thời gian đóng cửa"
                    },
                ]}
            >
                {getTimePicker(closeType)}
            </Form.Item>
            <Form.Item
                name="reason"
                label="Lý do đóng cửa"
                rules={[
                    {
                        required: true,
                        message: "Vui lòng nhập lý do đóng cửa"
                    },
                ]}
            >
                <Input.TextArea placeholder="Lý do..." />
            </Form.Item>
            <Form.Item style={{ textAlign: "center" }}>

                <Button type='primary' onClick={submit}>Đóng cửa</Button>
            </Form.Item>
        </Form>)
}
export default CloseParkingZoneForm