import { Form, Input, InputNumber, TimePicker, Upload, notification } from "antd";
import { UploadOutlined } from '@ant-design/icons'
import { useEffect, useState } from "react";
import useParkingZoneService from '@/services/parkingZoneService';
import dayjs from "dayjs";
import { v4 as uuidv4 } from "uuid";
const fileLimit = {
    maxLength: 8,
    maxSize: 3 * 1024 * 1024
}
const UpdateParkingZoneForm = ({ form, parkingZone }) => {

    const parkingZoneService = useParkingZoneService();

    const [images, setImages] = useState([]);
    useEffect(() => {
        const fieldsValue = {
            ...parkingZone,
            workingTime: [dayjs(parkingZone.workFrom, 'HH:mm:ss'), dayjs(parkingZone.workTo, 'HH:mm:ss')]
        }
        form.setFieldsValue(fieldsValue)
        parkingZoneService.getImageLink(parkingZone.id).then((res) => {
            const imageFilePromises = res.data.map(async (imgLink) => {
                const fileName = imgLink.substring(imgLink.lastIndexOf('/') + 1)
                return fetch(imgLink).then(async (res) => {
                    const blob = await res.blob()
                    const file = new File([blob], fileName, { type: "image/jpeg" })
                    return {
                        originFileObj: file,
                        lastModified: file.lastModified,
                        name: fileName,
                        percent: 0,
                        size: file.size,
                        type: file.type,
                        uid: uuidv4()
                    }
                });
            })
            Promise.all(imageFilePromises).then((res) => {
                setImages(res);
                form.setFieldValue("parkingZoneImages", { fileList: res })
            });

        });

    }, [])
    const notEmptyRule = {
        required: true,
        message: 'Vui lòng nhập đầy đủ thông tin',
    }
    return (
        <Form form={form}
            name="editForm"
            layout="vertical"
            onCha
        >
            <Form.Item
                name="name"
                label="Tên"
                rules={[notEmptyRule]}
            >
                <Input />
            </Form.Item>
            <Form.Item
                name="pricePerHour"
                label="Giá tiền (mỗi giờ)"
                rules={[notEmptyRule]}
            >
                <InputNumber
                    className="w-[100%]"
                    prefix="VND"
                    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                    parser={(value) => value.replace(/\$\s?|(,*)/g, '')}
                />
            </Form.Item>
            <Form.Item
                name="priceOverTimePerHour"
                label="Giá tiền quá giờ (mỗi giờ)"
                rules={[notEmptyRule]}
            >
                <InputNumber
                    className="w-[100%]"
                    prefix="VND"
                    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                    parser={(value) => value.replace(/\$\s?|(,*)/g, '')}
                />
            </Form.Item>
            <Form.Item
                name="slots"
                label="Số chỗ"
                rules={[notEmptyRule]}
            >
                <InputNumber
                    className="w-[100%]"
                    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                    parser={(value) => value.replace(/\$\s?|(,*)/g, '')}
                />
            </Form.Item>
            <Form.Item
                name="workingTime"
                label="Thời gian làm việc"
                rules={[notEmptyRule]}
            >
                <TimePicker.RangePicker
                    secondStep={60}
                    minuteStep={60}
                    placeholder={["Giờ mở cửa", "Giờ đóng cửa"]}
                    className="w-[100%]" />
            </Form.Item>
            <Form.Item
                name="parkingZoneImages"
                label={<p>Ảnh bãi đỗ xe (Tối đa {fileLimit.maxSize / 1024 / 1024}MB/ảnh) <i>**Các ảnh lớn hơn dung lượng cho phép sẽ bị xóa**</i></p>}
                rules={[notEmptyRule]}
            >
                <Upload
                    className="upload-list-inline"
                    accept="image/*"
                    listType="picture-card"
                    fileList={images}
                    onChange={(info) => {
                        setImages(info.fileList.filter((im) => im.size <= fileLimit.maxSize))
                    }}
                    multiple={true}
                    beforeUpload={() => false}
                    maxCount={fileLimit.maxLength}
                >
                    {
                        images.length < fileLimit.maxLength && (<div>
                            <UploadOutlined />
                            <div
                                style={{
                                    marginTop: 8,
                                }}
                            >
                                Upload
                            </div>
                        </div>)
                    }

                </Upload>
            </Form.Item>
        </Form>)
}
export default UpdateParkingZoneForm;