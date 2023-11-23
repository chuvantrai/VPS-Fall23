/* eslint-disable react-hooks/exhaustive-deps */
import { Form, Input, Modal, Row, Col, Button, InputNumber, Select, DatePicker } from 'antd';
import PropTypes from 'prop-types';
import { useEffect, useState } from 'react';

import { getAccountJwtModel } from '@/helpers';
import useParkingZoneService from '@/services/parkingZoneService'

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
const { RangePicker } = DatePicker;

function ModalAddCode({ open, confirmLoading, onCreate, onCancel }) {
  const [form] = Form.useForm();
  const account = getAccountJwtModel();
  const parkingZoneServices = useParkingZoneService()

  const [options, setOptions] = useState([]);
  const [parkingZoneIds, setParkingZoneIds] = useState([]);
  const [selectedDate, setSelectedDate] = useState('');

  useEffect(() => {
    parkingZoneServices.getApprovedParkingZoneByOwnerId(account.UserId)
      .then(res => {
        setOptions(res.data)
      })
  }, [])

  const makeCode = (length) => {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < length) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
      counter += 1;
    }
    return result;
  }

  const handleGenerateCode = () => {
    let code = makeCode(6)
    form.setFieldValue('code', code)
  }

  const handleChange = (value) => {
    setParkingZoneIds(value);
  };

  const handleChangeDate = (_, dateString) => {
    setSelectedDate(dateString)
  }

  return (
    <Modal
      open={open}
      confirmLoading={confirmLoading}
      title="Thêm mã mới"
      okText="Tạo"
      cancelText="Hủy"
      onCancel={() => {
        form.resetFields()
        onCancel()
      }}
      onOk={() => {
        form
          .validateFields()
          .then(async (values) => {
            await onCreate({ ...values, ownerId: account.UserId, parkingZoneIds: parkingZoneIds, selectedDate: selectedDate });
            form.resetFields();
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="modalAddCode" validateMessages={validateMessages}>
        <Form.Item label="Mã giảm giá">
          <Row gutter={8}>
            <Col span={18}>
              <Form.Item
                name="code"
                noStyle
                rules={[
                  {
                    required: true,
                  },
                ]}
              >
                <Input disabled />
              </Form.Item>
            </Col>
            <Col span={6}>
              <Button onClick={handleGenerateCode}>Tạo mã code</Button>
            </Col>
          </Row>
        </Form.Item>
        <Form.Item
          name='discount'
          label='Giảm giá'
          rules={[
            {
              required: true
            }
          ]}
        >
          <InputNumber
            addonAfter='%'
            style={{
              width: '100%',
            }}
            min={1}
            max={100}
          />
        </Form.Item>
        <Form.Item
          name='numberOfUses'
          label='Số lần sử dụng'
          rules={[
            {
              required: true
            },
          ]}
        >
          <InputNumber
            style={{
              width: '100%',
            }}
            min={1}
          />
        </Form.Item>
        <Form.Item
          name='parkingZoneIds'
          label='Danh sách bãi đỗ xe được áp dụng'
          rules={[
            {
              required: true
            },
          ]}
        >
          <Select
            mode="multiple"
            allowClear
            style={{
              width: '100%',
            }}
            onChange={handleChange}
            options={options}
          />
        </Form.Item>
        <Form.Item
          name='selectedDate'
          label='Thời gian sử dụng'
          rules={[
            {
              required: true
            },
          ]}
        >
          <RangePicker className='w-[100%]' onChange={handleChangeDate} />
        </Form.Item>
      </Form>
    </Modal>
  );
}

ModalAddCode.propTypes = {
  open: PropTypes.bool.isRequired,
  confirmLoading: PropTypes.bool.isRequired,
  onCreate: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ModalAddCode;
