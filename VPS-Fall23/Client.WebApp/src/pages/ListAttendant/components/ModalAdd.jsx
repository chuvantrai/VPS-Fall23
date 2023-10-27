import { useState, useCallback, useEffect } from 'react';
import { Form, Input, Modal, Select, notification } from 'antd';
import PropTypes from 'prop-types';

import AddressCascader from '@/components/cascader/AddressCascader';
import useParkingZoneService from '@/services/parkingZoneService';
import { getAccountJwtModel } from '@/helpers';

const validateMessages = {
  required: '${label} không được để trống!',
  types: {
    email: '${label} không đúng định dạng email!',
    number: '${label} không đúng định dạng số!',
  },
  number: {
    range: '${label} cần nằm giữa ${min} và ${max}',
  },
};

function ModalAdd({ open, onCreate, onCancel }) {
  const [form] = Form.useForm();
  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();

  const [selectedAddress, setSelectedAddress] = useState(null);
  const [parkingZoneList, setParkingZoneList] = useState([]);

  useEffect(() => {
    parkingZoneService
      .getAllParkingZoneByOwnerId(account.UserId)
      .then((res) => {
        setParkingZoneList(res.data);
      })
      .catch((err) => {
        notification.error({
          message: err.message,
        });
      });
  }, []);

  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa chỉ',
  };
  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ? selectedOptions[selectedOptions.length - 1] : null);
  }, []);

  return (
    <Modal
      open={open}
      title="Thêm tài khoản"
      centered
      okText="Tạo"
      cancelText="Hủy"
      onCancel={onCancel}
      onOk={() => {
        form
          .validateFields()
          .then((values) => {
            form.resetFields();
            onCreate({ ...values, communeId: selectedAddress?.id });
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="form_in_modal" validateMessages={validateMessages}>
        <Form.Item
          name="userName"
          label="Tên đăng nhập"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="password"
          label="Mật khẩu"
          rules={[
            {
              required: true,
            },
            {
              min: 6,
              message: 'Mật khẩu tối thiểu phải có 6 ký tự!',
            },
            {
              max: 12,
              message: 'Mật khẩu tối đa là 12 ký tự',
            },
          ]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item
          name="firstName"
          label="Tên"
          rules={[
            {
              required: true,
            },
            {
              max: 20,
              message: 'Tên tối đa chỉ được 20 ký tự!',
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="lastName"
          label="Họ"
          rules={[
            {
              required: true,
            },
            {
              max: 20,
              message: 'Họ tối đa chỉ được 20 ký tự!',
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="phoneNumber"
          label="Số điện thoại"
          rules={[
            {
              required: true,
            },
            {
              max: 10,
              message: 'Số điện thoại tối đa chỉ được 10 ký tự',
            },
            {
              pattern: /(0[3|5|7|8|9])+([0-9]{8})\b/g,
              message: 'Sai định dạng',
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="parkingZoneId"
          label="Bãi đỗ xe"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select
            showSearch
            placeholder="Search to Select"
            optionFilterProp="children"
            filterOption={(input, option) => (option?.label ?? '').includes(input)}
            filterSort={(optionA, optionB) =>
              (optionA?.label ?? '').toLowerCase().localeCompare((optionB?.label ?? '').toLowerCase())
            }
            options={parkingZoneList}
          />
        </Form.Item>
        <Form.Item name="communeId" label="Địa chỉ">
          <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
        </Form.Item>
        <Form.Item name="address" label="Địa chỉ chi tiết">
          <Input.TextArea />
        </Form.Item>
      </Form>
    </Modal>
  );
}

ModalAdd.propTypes = {
  open: PropTypes.bool.isRequired,
  onCreate: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ModalAdd;
