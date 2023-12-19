import './modalReport.module.scss';
import { Button, Form, Input, Modal, Select } from 'antd';
import reportTypeEnum from '@/helpers/reportTypeEnum.js';
import { useState } from 'react';
import optionsReportType from '@/helpers/optionsReportType.js';
import GetAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import userRoleEnum from '@/helpers/userRoleEnum.js';
import reportServices from '@/services/reportServices.js';
import classNames from 'classnames/bind.js';
import styles from '@/pages/Test/Test.module.scss';
import TextArea from 'antd/es/input/TextArea.js';

const cx = classNames.bind(styles);

const templateDriver = (
  <>
    <Form.Item
      label='Email của bạn'
      name='email'
      rules={[
        { required: true, message: 'Vui lòng nhập email của bạn' },
        {
          type: 'email',
          message: 'Email không hợp lệ',
        },
      ]}
    >
      <Input placeholder='Email'></Input>
    </Form.Item>
    <Form.Item
      label='Số ĐT'
      name='phone'
      rules={[
        {
          required: true,
          message: 'Hãy điền số điện thoại của bạn!',
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
      <Input placeholder='Số điện thoại'></Input>
    </Form.Item>
  </>
);

const templatePaymentCode = (
  <>
    <Form.Item
      label='Mã giao dịch'
      name='paymentCode'
      rules={[
        { required: true, message: 'Vui lòng nhập email của bạn' },
      ]}
    >
      <Input placeholder='Mã giao dịch'></Input>
    </Form.Item>
  </>
);

const ModalReport = ({ contentBtn }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  let isDriver = false;

  const [form] = Form.useForm();
  let options = optionsReportType;
  const account = GetAccountJwtModel();
  if (account?.RoleId === userRoleEnum.OWNER) {
      options = options.filter(option =>
          option.value !== reportTypeEnum.REQUEST_TRANSACTION_REFUND &&
          option.value !== reportTypeEnum.TRANSACTION_ERROR &&
          option.value !== reportTypeEnum.REPORT_PARKING_ZONE);
  }
  if (account === null) {
    options = options.filter(option =>
      option.value === reportTypeEnum.REQUEST_TRANSACTION_REFUND ||
      option.value === reportTypeEnum.TRANSACTION_ERROR);
    isDriver = true;
  }
  const defaultValueSelect = options[0].value;

  const [isTypeRefund, setIsTypeRefund] =
    useState(defaultValueSelect === reportTypeEnum.REQUEST_TRANSACTION_REFUND
      || defaultValueSelect === reportTypeEnum.TRANSACTION_ERROR);
  const onChangeReportType = (value) => {
    setIsTypeRefund(value === reportTypeEnum.REQUEST_TRANSACTION_REFUND || value === reportTypeEnum.TRANSACTION_ERROR);
  };

  const reportService = reportServices();
  const onFinish = (value) => {
    value.type = value.type ?? reportTypeEnum.ERROR_DISPLAY;
    reportService.createReport(value);
  };

  return (
    <>
      <span onClick={showModal}>
        {contentBtn}
      </span>

      <Modal
        confirmLoading={true}
        title='Tạo báo cáo'
        open={isModalOpen}
        okButtonProps={{
          style: {
            backgroundColor: '#1677ff',
            display: 'none',
          },
        }}
        cancelButtonProps={{
          style: {
            display: 'none',
          },
        }}
        onCancel={handleCancel}>
        {/*START form report*/}
        <Form
          labelCol={{ span: 6 }}
          wrapperCol={{ span: 17 }}
          form={form}
          onFinish={onFinish}
        >
          <Form.Item
            label='Loại báo cáo'
            name='type'
          >
            <Select
              className={cx('min-w-[230px]')}
              defaultValue={defaultValueSelect}
              style={{ width: 120 }}
              options={options}
              onChange={onChangeReportType}
            />
          </Form.Item>
          {isDriver === true ? templateDriver : <></>}
          {isTypeRefund === true ? templatePaymentCode : <></>}
          <Form.Item
            label='Nội dung'
            name='content'
            rules={[
              {
                required: true,
                message: 'Không để trống nội dung!',
              },
              {
                max: 1000,
                message: 'Nội dung tối đa 1000 ký tự!',
              },
            ]}
          >
            <TextArea className={'max-h-[97px!important] min-h-[97px!important] w-full'} rows={4}
                      placeholder='Nội dung tối đa 1000 ký tự!' />
          </Form.Item>
          <Form.Item className={('flex justify-center m-0')}>
            <Button className={('bg-[#1890FF] w-[200%]')} type='primary' htmlType={'submit'}>
              Gửi
            </Button>
          </Form.Item>
        </Form>
        {/*END form report*/}
      </Modal>
    </>
  );
};

export default ModalReport;
