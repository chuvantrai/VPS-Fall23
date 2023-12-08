/* eslint-disable react-hooks/exhaustive-deps */
import { Form, Input, Modal } from 'antd';
import PropTypes from 'prop-types';

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

function ModalBlockReason({ open, accountId, onBlock, onCancel }) {
  const [form] = Form.useForm();

  return (
    <Modal
      open={open}
      title="Khóa tài khoản"
      cancelText="Hủy"
      onCancel={onCancel}
      onOk={() => {
        form
          .validateFields()
          .then((values) => {
            form.resetFields();
            onBlock({ ...values, isBlock: true, accountId: accountId });
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="form_in_modal" validateMessages={validateMessages}>
        <Form.Item name="blockReason"
          rules={[
            {
              required: true,
              message: 'Lý do không được để trống!'
            }
          ]}
        >
          <Input.TextArea placeholder='Nhập lý do vào đây...' />
        </Form.Item>
      </Form>
    </Modal>
  );
}

ModalBlockReason.propTypes = {
  open: PropTypes.bool.isRequired,
  accountId: PropTypes.string.isRequired,
  onBlock: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ModalBlockReason;
