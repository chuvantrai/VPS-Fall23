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

function ReplyModal({ open, feedbackId, onCreate, onCancel }) {
  const [form] = Form.useForm();

  form.setFieldsValue({
    feedbackId: feedbackId,
  });

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
            onCreate(values);
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}
    >
      <Form form={form} layout="vertical" name="reply_form_modal" validateMessages={validateMessages}>
        <Form.Item name="feedbackId" hidden>
          <Input />
        </Form.Item>
        <Form.Item
          name="content"
          label="Nội dung phản hồi"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input.TextArea placeholder="Nội dung..." />
        </Form.Item>
      </Form>
    </Modal>
  );
}

ReplyModal.propTypes = {
  open: PropTypes.bool.isRequired,
  feedbackId: PropTypes.string.isRequired,
  onCreate: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ReplyModal;
