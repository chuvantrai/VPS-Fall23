import { Form, Modal, Input, Radio, Space, DatePicker } from 'antd';
import PropTypes from 'prop-types';
import { useState } from 'react';

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

function CloseModalForm({ open, parkingZoneId, onClose, onCancel }) {
  const [form] = Form.useForm();

  const [value, setValue] = useState(1);
  const [selectedTime, setSelectedTime] = useState('');

  const onChangeRadio = (e) => {
    setValue(e.target.value);
  };
  const onChangeDate = (value, dateString) => {
    setSelectedTime(dateString);
  };

  return (
    <Modal
      open={open}
      title="Đóng cửa bãi đỗ xe"
      centered
      okText="Gửi"
      cancelText="Hủy"
      onCancel={onCancel}
      onOk={() => {
        form
          .validateFields()
          .then((values) => {
            form.resetFields();
            onClose({ ...values, closeTime: selectedTime, parkingZoneId: parkingZoneId });
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="closeModalForm" validateMessages={validateMessages}>
        <Form.Item name="parkingZoneId" hidden>
          <Input value={parkingZoneId} />
        </Form.Item>
        <Form.Item>
          <Radio.Group onChange={onChangeRadio} value={value}>
            <Space>
              <Radio value={1}>Đóng cửa vĩnh viễn</Radio>
              <Radio value={2}>Đóng cửa một thời gian</Radio>
            </Space>
          </Radio.Group>
        </Form.Item>
        {value === 1 && (
          <Form.Item
            name="closeFrom"
            label="Thời gian đóng cửa"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <DatePicker onChange={onChangeDate} />
          </Form.Item>
        )}
        {value === 2 && (
          <Form.Item
            name="closeTime"
            label="Thời gian đóng cửa"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <RangePicker format="YYYY-MM-DD" onChange={onChangeDate} />
          </Form.Item>
        )}
        <Form.Item
          name="reason"
          label="Lý do đóng cửa"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input.TextArea placeholder="Lý do..." />
        </Form.Item>
      </Form>
    </Modal>
  );
}

CloseModalForm.propTypes = {
  open: PropTypes.bool.isRequired,
  parkingZoneId: PropTypes.string.isRequired,
  onClose: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default CloseModalForm;
