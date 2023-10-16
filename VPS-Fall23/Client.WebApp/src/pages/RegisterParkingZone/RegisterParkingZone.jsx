import { Button, Form, Input, InputNumber, Modal, Upload } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { useState, useCallback } from 'react';

import AddressCascader from '@/components/cascader/AddressCascader';
import useParkingZoneService from '@/services/parkingZoneService';

const layout = {
  labelCol: {
    span: 8,
  },
  wrapperCol: {
    span: 16,
  },
};

const validateMessages = {
  required: '${label} is required!',
  types: {
    email: '${label} is not a valid email!',
    number: '${label} is not a valid number!',
  },
  number: {
    range: '${label} must be between ${min} and ${max}',
  },
};

const getBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

const RegisterParkingZone = () => {
  const parkingZoneService = useParkingZoneService();

  const [previewOpen, setPreviewOpen] = useState(false);
  const [previewImage, setPreviewImage] = useState('');
  const [previewTitle, setPreviewTitle] = useState('');
  const [fileList, setFileList] = useState([]);
  const [selectedAddress, setSelectedAddress] = useState(null);
  const [validateStatus, setValidateStatus] = useState('null');
  const [help, setHelp] = useState('');

  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa chỉ',
  };

  const uploadButton = (
    <div>
      <PlusOutlined />
      <div
        style={{
          marginTop: 8,
        }}
      >
        Upload
      </div>
    </div>
  );

  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ? selectedOptions[selectedOptions.length - 1] : null);
    setValidateStatus('');
    setHelp('');
  }, []);

  const handleCancel = () => setPreviewOpen(false);

  const handlePreview = async (file) => {
    if (!file.url && !file.preview) {
      file.preview = await getBase64(file.originFileObj);
    }
    setPreviewImage(file.url || file.preview);
    setPreviewOpen(true);
    setPreviewTitle(file.name || file.url.substring(file.url.lastIndexOf('/') + 1));
  };

  const handleChange = ({ fileList: newFileList }) => setFileList(newFileList);

  const onFinish = (values) => {
    if (!selectedAddress) {
      setValidateStatus('error');
      setHelp('Vui lòng chọn địa chỉ của bãi đỗ xe');
    } else {
      values = { ...values, parkingZoneImages: fileList, communeId: selectedAddress?.id };

      const formData = new FormData();
      formData.append('ownerId', '290E1476-AA4F-4BD6-8A23-E7167E1D0417');
      formData.append('name', values.name);
      formData.append('pricePerHour', values.pricePerHour);
      formData.append('priceOverTimePerHour', values.priceOverTimePerHour);
      formData.append('slots', values.slots);
      formData.append('communeId', values.communeId);
      formData.append('detailAddress', values.detailAddress);
      values.parkingZoneImages.forEach((item) => {
        formData.append('parkingZoneImages', item.originFileObj);
      });

      parkingZoneService.register(formData);
    }
  };

  return (
    <>
      <Form
        {...layout}
        className="pt-[24px] pb-[24px]"
        name="nest-messages"
        onFinish={onFinish}
        style={{
          width: '100%',
          minHeight: '484px',
          paddingLeft: '64px',
        }}
        validateMessages={validateMessages}
      >
        <Form.Item
          className="pb-[24px] m-0"
          name="name"
          label="Tên"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input className="w-[484px] h-[32px]" placeholder="Đặt tên cho parking zone" />
        </Form.Item>
        <Form.Item
          className="pb-[24px] m-0"
          name="pricePerHour"
          label="Giá tiền mỗi giờ"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <InputNumber className="w-[484px] h-[32px]" prefix="VND" />
        </Form.Item>
        <Form.Item
          className="pb-[24px] m-0"
          name="priceOverTimePerHour"
          label="Giá tiền quá giờ"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <InputNumber className="w-[484px] h-[32px]" prefix="VND" />
        </Form.Item>
        <Form.Item
          className="pb-[24px] m-0"
          name="slots"
          label="Slots"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <InputNumber className="w-[484px] h-[32px]" placeholder="Số slots của parking zone" />
        </Form.Item>
        <Form.Item
          name="communeId"
          label="Địa chỉ"
          className="pb-[24px] m-0"
          validateStatus={validateStatus}
          help={help}
        >
          <div className="w-[484px] h-[32px]">
            <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
          </div>
        </Form.Item>
        <Form.Item
          name={'detailAddress'}
          label="Địa chỉ cụ thể"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <div className="w-[484px]">
            <Input.TextArea
              placeholder="Địa chỉ cụ thể"
              style={{
                height: '76px',
              }}
            />
          </div>
        </Form.Item>
        <Form.Item
          name="parkingZoneImages"
          label="Ảnh bãi đỗ xe"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <div className="w-[484px]">
            <Upload
              accept="image/*"
              listType="picture-card"
              fileList={fileList}
              onPreview={handlePreview}
              onChange={handleChange}
              beforeUpload={() => false}
            >
              {fileList.length >= 8 ? null : uploadButton}
            </Upload>
          </div>
        </Form.Item>
        <Form.Item
          className="m-0"
          wrapperCol={{
            ...layout.wrapperCol,
            offset: 8,
          }}
        >
          <Button className="bg-[#1677ff]" type="primary" htmlType="submit">
            Submit
          </Button>
        </Form.Item>
      </Form>
      <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={handleCancel}>
        <img
          alt="example"
          style={{
            width: '100%',
          }}
          src={previewImage}
        />
      </Modal>
    </>
  );
};
export default RegisterParkingZone;
