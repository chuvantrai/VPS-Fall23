import { DatePicker, Form, Input, Radio, Space } from "antd"
import dayjs from "dayjs";
import { useEffect, useState } from "react"

const CloseParkingZoneForm = ({ form }) => {

    const [closeType, setCloseType] = useState(2);
    const getDisabledDate = (value) => {
        return value && value <= dayjs().endOf('day')
    }

    const getTimePicker = (closeType) => {
        form.setFieldValue("closeTime", undefined);
        switch (closeType) {
            case 1: return <DatePicker disabledDate={getDisabledDate} placeholder="Từ ngày"></DatePicker>;
            case 2:
            default: return <DatePicker.RangePicker disabledDate={getDisabledDate} placeholder={["Từ ngày", "Đến ngày"]}></DatePicker.RangePicker>
        }
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
                name="closeTime"
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
        </Form>)
}
export default CloseParkingZoneForm