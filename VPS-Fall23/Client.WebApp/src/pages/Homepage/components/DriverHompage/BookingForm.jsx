import { Button, DatePicker, Form, Input, Modal, Space } from 'antd';
import dayjs from "dayjs"

const range = (start, end) => {
    const result = [];
    for (let i = start; i < end; i++) {
        result.push(i);
    }
    return result;
};

const BookingForm = ({ parkingZone, onSubmitCallback, onCloseCallback }) => {

    const [form] = Form.useForm();

    const getDateFromNow = (current) => {
        return current && current < dayjs().startOf('day')
    }
    const getDisabledTime = (current, type) => {
        return {
            disabledHours: () => range(0, 24).splice(0, dayjs().hour() + 1),
        }
    }
    const onSubmitClick = () => {
        form.validateFields().then(form => {
            let parkingTransaction = {
                ...form,
                parkingZoneId: parkingZone.id,
                checkinAt: form.checkinTime[0],
                checkoutAt: form.checkinTime[1],
                licensePlate: `${form.licensePlatePre.trim()}-${form.licensePlateMid.trim()}\n${form.licensePlateEnd.trim()}`.toUpperCase()
            }
            onSubmitCallback(parkingTransaction);
        });
    }
    return (
        <Form
          labelCol={{ span: 6 }}
          wrapperCol={{ span: 14 }}
            form={form}
        >
            <Form.Item
                label="Email của bạn"
                name="email"
                rules={[
                    { required: true, message: "Vui lòng nhập email của bạn" },
                    {
                        type: "email",
                        message: "Email không hợp lệ"
                    }
                ]}
            >
                <Input></Input>
            </Form.Item>
            <Form.Item
                label="Số điện thoại"
                name="phone"
                rules={
                    [
                        {
                            required: true, message: "Vui lòng nhập số điện thoại của bạn"
                        },
                        {
                            pattern: /^\d+$/gm,
                            message: "Số điện thoại không hợp lệ"
                        }
                    ]
                }
            >
                <Input></Input>
            </Form.Item>
            <Form.Item
                label="Biển số xe"
                rules={[{ required: true }]}
            >
                <Space.Compact>
                    <Form.Item
                        name="licensePlatePre"
                        noStyle
                        rules={[
                            { required: true, message: "Vui lòng nhập biển số xe" },
                            { pattern: /^[0-9]{2}/gm, message: "" }
                        ]}
                    >
                        <Input style={{ width: '30%' }} required addonAfter="-" />
                    </Form.Item>
                    <Form.Item
                        name="licensePlateMid"
                        noStyle
                        rules={[
                            { required: true, message: "Vui lòng nhập biển số xe" },
                            { pattern: /^[0-9A-Za-z]/gm, message: "" }
                        ]}
                    >
                        <Input style={{ width: '30%' }} required />
                    </Form.Item>
                    <Form.Item
                        name="licensePlateEnd"
                        noStyle
                        rules={[
                            { required: true, message: "Vui lòng nhập biển số xe" }
                        ]}
                    >
                        <Input style={{ width: '40%' }} type="number" required />
                    </Form.Item>
                </Space.Compact>
            </Form.Item>
            <Form.Item
                label="Thời gian ra/vào"
                name="checkinTime"
                rules={[{ required: true, message: "Vui lòng chọn thời gian ra vào" }]}
            >
                <DatePicker.RangePicker
                    disabledDate={getDateFromNow}
                    disabledTime={getDisabledTime}
                    showTime={true}
                    popupClassName={'popupDatePicker'}
                    format="YYYY-MM-DD HH:mm"
                    name="checkinTime"
                />
            </Form.Item>
            <Form.Item className={('flex justify-center m-0')}>
                <Button className={('bg-[#1890FF] w-[200%]')} type='primary' onClick={onSubmitClick}>
                    Đặt chỗ
                </Button>
            </Form.Item>
        </Form>
    )
}

export default BookingForm  
