/* eslint-disable react-hooks/exhaustive-deps */
import { Form, Input, Modal, Row, Col, Button, InputNumber, Select, DatePicker } from 'antd';
import PropTypes from 'prop-types';
import { useEffect, useRef, useState } from 'react';
import dayjs from 'dayjs';

import { getAccountJwtModel } from '@/helpers';
import useParkingZoneService from '@/services/parkingZoneService'
import usePromoCodeServices from '@/services/promoCodeServices'

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

function ModalDetailCode({ open, promoCodeId, confirmLoading, onUpdate, onCancel }) {
  const [form] = Form.useForm();

  const account = getAccountJwtModel();
  const parkingZoneServices = useParkingZoneService()
  const promoCodeServices = usePromoCodeServices()

  const [options, setOptions] = useState([]);
  const [parkingZoneIds, setParkingZoneIds] = useState([]);
  const [selectedDate, setSelectedDate] = useState('');

  useEffect(() => {
    parkingZoneServices.getApprovedParkingZoneByOwnerId(account.UserId)
      .then(res => {
        setOptions(res.data)
      })
  }, [])

  const firstRender = useRef(true)
  useEffect(() => {
    if (firstRender.current) {
      firstRender.current = false;
      return;
    }
    promoCodeServices.getPromoCodeDetails(promoCodeId)
      .then(res => {
        let promoCodeObj = res.data
        let parkingZoneLst = promoCodeObj.parkingZoneIdLst

        form.setFieldsValue({
          discount: promoCodeObj.discount,
          parkingZoneIds: parkingZoneLst,
          selectedDate: [dayjs(promoCodeObj.fromDate), dayjs(promoCodeObj.toDate)]
        })

        setParkingZoneIds(parkingZoneLst)
        setSelectedDate([promoCodeObj.fromDate, promoCodeObj.toDate])
      })
  }, [promoCodeId, open])

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
      title="Thông tin chi tiết mã giảm giá"
      okText="Lưu"
      cancelText="Hủy"
      onCancel={() => {
        form.resetFields()
        onCancel()
      }}
      onOk={() => {
        form
          .validateFields()
          .then(async (values) => {
            await onUpdate(
              {
                ...values,
                ownerId: account.UserId,
                parkingZoneIds: parkingZoneIds,
                selectedDate: selectedDate,
                promoCodeId: promoCodeId
              }
            );
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="modalDetailCode" validateMessages={validateMessages}>
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

ModalDetailCode.propTypes = {
  open: PropTypes.bool.isRequired,
  confirmLoading: PropTypes.bool.isRequired,
  promoCodeId: PropTypes.string.isRequired,
  onUpdate: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ModalDetailCode;
